using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.Metadata;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using LibraryManagementWpf.Models;
using static System.Net.WebRequestMethods;
using LibraryManagementWpf.ViewModel.Common;
using static WebView2.DOM.HTMLInputElement;
using Microsoft.EntityFrameworkCore;
using LibraryManagementWpf.View.Admin;

namespace LibraryManagementWpf.EmailSender
{

    public class EmailSender
    {
        private const string fromEmail = "hanvhe171012@fpt.edu.vn";
        private const string emailPassword = "gpuxngffobzutsfq";
        private string smtpHost = "smtp.gmail.com";
        private const int smtpPort = 587;
        private bool enableSsl = true;
        private string filePathForm = @"LibraryManagementWpf\EmailSender\EmailSenderForm.html";
        public EmailSender()
        {
        }
        public async Task SendEmail(string toEmail, string subject, string replaceContentType, int borrowId = 0)
        {
            try
            {
                string htmlContent = getEmailSenderFormat(toEmail, replaceContentType, borrowId);

                var fromAddress = new MailAddress(fromEmail, "PRN221_SE1745-NET_G6");
                var toAddress = new MailAddress(toEmail);

                var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = smtpPort,
                    EnableSsl = enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail, emailPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = htmlContent,
                    IsBodyHtml = true  
                })
                {
                    await smtp.SendMailAsync(message);
				}

                Trace.TraceInformation("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error sending email: " + ex.Message);
            }
        }

		


        public string getEmailSenderFormat(string toEmail, string replaceContentType,int borrowId)
        {
            StringBuilder content = new StringBuilder();
            var resourceUri = new Uri("pack://application:,,,/EmailSender/EmailSenderForm.html", UriKind.RelativeOrAbsolute);
            var streamResourceInfo = Application.GetResourceStream(resourceUri);
            if (streamResourceInfo != null)
            {
                using (StreamReader sr = new StreamReader(streamResourceInfo.Stream))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        content.Append(line).Append("\n");
                    }
                }
            }
            string updatedContent = string.Empty;
            if (replaceContentType == "resetPasswordCode")
            {
                updatedContent = replaceAttributeForNewPassword(content.ToString(), toEmail);
				MessageBox.Show("Mật khẩu mới đã được gửi đến email của bạn. Đừng quên thay đổi mật khẩu ngay sau khi đăng nhập.");
			}
			else if (replaceContentType == "sendOTPCode")
            {
                updatedContent = replaceAttributeForSendOTPCode(content.ToString(), toEmail);
				MessageBox.Show("Vui lòng kiểm tra email của bạn nhập mã OTP để hoàn tất xác thực.");
			} else if(replaceContentType == "sendBill")
            {
                updatedContent = replaceAttributeForSendBill(content.ToString(), toEmail);
            }
            else if (replaceContentType == "fineNotify")
            {
                updatedContent = replaceAttributeForFineNotify(content.ToString(), toEmail, borrowId);
            }
            return updatedContent;

        }
        public string replaceAttributeForFineNotify(string content, string toEmail, int borrowId)
        {
            StringBuilder fineDetailsBuilder = new StringBuilder();

            try
            {
                using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
                {
                    var fineInfo = _context.Fines.Include(b => b.Borrow).ThenInclude(b => b.Book).Where(b => b.Borrow.BorrowId == borrowId)
                        .FirstOrDefault();
                    fineDetailsBuilder.Append($"<div style='padding: 20px; background-color: #f9f9f9; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>" +
                                              $"<h2 style='font-size: 20px; font-weight: 600; color: #1f1f1f;'>Thông tin mượn sách</h2>" +
                                              $"<p style='font-size: 16px; font-weight: 500; color: #333;'>" +
                                              $"<strong style='color: #ba3d4f;'>Tên sách:</strong> {fineInfo.Borrow.Book.Title} <br/>" +
                                              $"<strong style='color: #ba3d4f;'>Ngày mượn:</strong> {fineInfo.Borrow.BorrowDate?.ToString("dd/MM/yyyy")} <br/>" +
                                              $"<strong style='color: #ba3d4f;'>Ngày đến hạn:</strong> {fineInfo.Borrow.DueDate?.ToString("dd/MM/yyyy")} <br/>" +
                                              $"<strong style='color: #ba3d4f;'>Phạt:</strong> {fineInfo.FineAmount} VND <br/>" +
                                              $"<strong style='color: #ba3d4f;'>Loại phạt:</strong> {fineInfo.FineType} <br/>" +
                                              $"</p>" +
                                              $"</div>");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Đã có lỗi xảy ra. Vui lòng thử lại sau.");
            }

            string fineDetails = fineDetailsBuilder.ToString();
            string updatedContent = content.Replace("XXXContentXXX", fineDetails);

            string time = DateTime.Now.ToString("HH:mm");
            string date = DateTime.Now.ToString("ddMMM,yyyy");

            updatedContent = updatedContent.Replace("XXXtimeSentFormXXX", time);
            updatedContent = updatedContent.Replace("XXXdateSentFormXXX", date);
            updatedContent = updatedContent.Replace("XXXrecipient's_emailXXX", toEmail);
            updatedContent = updatedContent.Replace("XXXgetFormTitleXXX", "Fine Notify");
            updatedContent = updatedContent.Replace("XXXgetNotificationXXX",
             "Cảm ơn bạn đã sử dụng Hệ thống Thư viện FPTU. Yêu cầu bạn sắp xếp thời gian đến thanh toán tiền phạt càng sớm càng tốt để tránh tăng tiền phạt");

            return updatedContent;
        }
        public string replaceAttributeForSendBill(string content, string toEmail)
        {
            StringBuilder fineDetailsBuilder = new StringBuilder();

            try
            {
                using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
                {
                    var currentUser = SessionManager.CurrentUser;
                    if (currentUser != null)
                    {
                        var userFines = _context.BorrowBooks
                           .Where(b => b.UserId == currentUser.UserId)
                           .SelectMany(b => b.Fines).Where(b => b.Status == 0)
                           .ToList();
                        decimal totalFineAmount = userFines.Sum(f => f.FineAmount ?? 0);
                        fineDetailsBuilder.AppendLine("<table style='width:100%; border-collapse: collapse;'>" +
                                                        "<tr style='background-color: #f2f2f2;'>" +
                                                        "<th style='border: 1px solid #ddd; padding: 8px;'>Fine ID</th>" +
                                                        "<th style='border: 1px solid #ddd; padding: 8px;'>Fine Type</th>" +
                                                        "<th style='border: 1px solid #ddd; padding: 8px;'>Amount</th>" +
                                                        "</tr>");

                        foreach (var fine in userFines)
                        {
                            fineDetailsBuilder.AppendLine("<tr>");
                            fineDetailsBuilder.AppendLine($"<td style='border: 1px solid #ddd; padding: 8px;'>{fine.FineId}</td>");
                            fineDetailsBuilder.AppendLine($"<td style='border: 1px solid #ddd; padding: 8px;'>{fine.FineType}</td>");
                            fineDetailsBuilder.AppendLine($"<td style='border: 1px solid #ddd; padding: 8px;'>{fine.FineAmount:C}</td>");
                            fineDetailsBuilder.AppendLine("</tr>");
                        }

                        fineDetailsBuilder.AppendLine("<tr style='font-weight: bold;'>" +
                            "<td style='border: 1px solid #ddd; padding: 8px;' colspan='3'>Total</td>" +
                            $"<td style='border: 1px solid #ddd; padding: 8px;'>{totalFineAmount:C}</td>" +
                            "</tr>");

                        fineDetailsBuilder.AppendLine("</table>");
                    }                    
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Đã có lỗi xảy ra. Vui lòng thử lại sau.");
            }
            string fineDetails = fineDetailsBuilder.ToString();

            string time = DateTime.Now.ToString("HH:mm");
            string date = DateTime.Now.ToString("ddMMM,yyyy");

            string updatedContent = content.Replace("XXXContentXXX", fineDetails);
            updatedContent = updatedContent.Replace("XXXtimeSentFormXXX", time);
            updatedContent = updatedContent.Replace("XXXdateSentFormXXX", date);
            updatedContent = updatedContent.Replace("XXXrecipient's_emailXXX", toEmail);
            updatedContent = updatedContent.Replace("XXXgetFormTitleXXX", "Bill phạt lượt mượn");
            updatedContent = updatedContent.Replace("XXXgetNotificationXXX",
               "Cuốn sách bạn mượn đã quá hạn hoặc mất, yêu cầu bạn thu xếp thời gian đến trực tiếp để đóng tiền phạt. " +
               "Kiểm tra lại nếu bạn không mượn quá hạn hoặc làm mất để thông báo lại thủ thư để xử lý.");
            return updatedContent;
        }
        public string replaceAttributeForSendOTPCode(string content, string toEmail) {
            string otpCode = generateRandomOTPCode();
            try
            {
				using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
				{
					var userOTP = _context.Users.SingleOrDefault(x => x.Email == toEmail);
					if (userOTP != null)
					{
						userOTP.VerifyCode = otpCode.ToString();
						userOTP.ExpirationCode = DateTime.UtcNow.AddMinutes(2).ToLocalTime();
						_context.Update(userOTP);
						_context.SaveChanges();
					}
				}
			}
			catch(Exception e)
            {
                MessageBox.Show("Đã có lỗi xảy ra. Vui lòng thử lại sau.");
            }
			
			string updatedContent = content.Replace("XXXContentXXX", otpCode);

            string time = DateTime.Now.ToString("HH:mm");
            string date = DateTime.Now.ToString("ddMMM,yyyy");

            updatedContent = updatedContent.Replace("XXXtimeSentFormXXX", time);
            updatedContent = updatedContent.Replace("XXXdateSentFormXXX", date);
            updatedContent = updatedContent.Replace("XXXrecipient's_emailXXX", toEmail);
            updatedContent = updatedContent.Replace("XXXgetFormTitleXXX", "Mã OTP");
			updatedContent = updatedContent.Replace("XXXgetNotificationXXX",
	         "Cảm ơn bạn đã sử dụng Hệ thống Thư viện FPTU. Nếu bạn không yêu cầu mã OTP này, vui lòng bỏ qua tin nhắn này. " +
	         "Sử dụng mã OTP sau để hoàn tất thủ tục lấy mật khẩu mới của bạn. Vì lý do bảo mật, mã OTP có hiệu lực trong " +
	         "<span style=\"font-weight: 600; color: #1f1f1f;\">2 phút</span>. " +
	         "Mã OTP này chỉ có hiệu lực một lần và không nên chia sẻ với bất kỳ ai.");

			return updatedContent;
        }

        public string replaceAttributeForNewPassword(string content, string toEmail) {
            string newPassword = generateRandomPassword();
			using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
			{
                
				var userResetPwd = _context.Users.SingleOrDefault(u => u.Email == toEmail);
				if (userResetPwd != null)
				{
					userResetPwd.Password = AuthenViewModel.Encrypt(newPassword);
					_context.Update(userResetPwd);
					_context.SaveChanges();
				}
			}
			string updatedContent = content.Replace("XXXContentXXX", newPassword);

            string time = DateTime.Now.ToString("HH:mm");
            string date = DateTime.Now.ToString("ddMMM,yyyy");

            updatedContent = updatedContent.Replace("XXXtimeSentFormXXX", time);
            updatedContent = updatedContent.Replace("XXXdateSentFormXXX", date);
            updatedContent = updatedContent.Replace("XXXrecipient's_emailXXX", toEmail);
            updatedContent = updatedContent.Replace("XXXgetFormTitleXXX", "Mật khẩu mới");
			updatedContent = updatedContent.Replace("XXXgetNotificationXXX",
	           "Cảm ơn bạn đã sử dụng Hệ thống Thư viện FPTU. Nếu bạn không yêu cầu điều này, vui lòng bỏ qua tin nhắn này. " +
	           "Mật khẩu này có hiệu lực và không nên chia sẻ với bất kỳ ai.");

			return updatedContent;
        }
        private static string generateRandomPassword()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()-_+=<>?";

            string allChars = upperCase + lowerCase + digits + specialChars;
            Random random = new Random();

            string password = new string(Enumerable.Repeat(upperCase, 1)
            .Select(s => s[random.Next(s.Length)]).ToArray()) +
            new string(Enumerable.Repeat(lowerCase, 1)
            .Select(s => s[random.Next(s.Length)]).ToArray()) +
            new string(Enumerable.Repeat(digits, 1)
            .Select(s => s[random.Next(s.Length)]).ToArray()) +
            new string(Enumerable.Repeat(specialChars, 1)
            .Select(s => s[random.Next(s.Length)]).ToArray());

            password += new string(Enumerable.Repeat(allChars, 12 - 4)
            .Select(s => s[random.Next(s.Length)]).ToArray());

            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

        public static string generateRandomOTPCode()
        {
            Random random = new Random();
            string otpCode = string.Empty;

            for (int i = 0; i < 6; i++)
            {
                otpCode += random.Next(0, 10).ToString(); 
            }

            return otpCode;
        }
        
    }
}
