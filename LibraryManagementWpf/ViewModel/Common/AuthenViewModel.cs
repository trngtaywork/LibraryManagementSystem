using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Admin;
using LibraryManagementWpf.View.Librarian;
using LibraryManagementWpf.View.Common;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LibraryManagementWpf.View.Student;
using System.Net.Mail;
using System.Net;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using WebView2.DOM;
using LibraryManagementWpf.ViewModel.Student;
using LibraryManagementWpf.ViewModel.Admin;
namespace LibraryManagementWpf.ViewModel.Common
{
	public class AuthenViewModel : BaseViewModel
	{
		private DispatcherTimer _timer;
		private TimeSpan _timeLeft;
		int otp;
		public ICommand AuthenCommand { get; set; }
		public ICommand PasswordChangedCommand { get; set; }
		public ICommand RePasswordChangedCommand { get; set; }
		public ICommand RegisterViewCommand { get; set; }
		public ICommand RegisterCommand { get; set; }
		public ICommand LoginViewCommand { get; set; }
		public ICommand ResendOTPCommand { get; set; }
		public ICommand SubmitOTPCommand { get; set; }
		public ICommand ForgotPasswordCommand { get; set; }
		public ICommand RequestPasswordResetCommand { get; set; }

		
		private string _OTP;
		public string OTP { get => _OTP; set { _OTP = value; OnPropertyChanged(); } }
		private string _Countdown;
		public string Countdown { get => _Countdown; set { _Countdown = value; OnPropertyChanged(); } }
		private string _Email;
		public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }
		private string _StudentCode;
		public string StudentCode { get => _StudentCode; set { _StudentCode = value; OnPropertyChanged(); } }
		private string _Password;
		public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
		private string _RePassword;
		public string RePassword { get => _RePassword; set { _RePassword = value; OnPropertyChanged(); } }
		private string _errorEmail;
		public string errorEmail { get => _errorEmail; set { _errorEmail = value; OnPropertyChanged(); } }
		private string _errorStudentCode;
		public string errorStudentCode { get => _errorStudentCode; set { _errorStudentCode = value; OnPropertyChanged(); } }
		private string _errorPassword;
		public string errorPassword { get => _errorPassword; set { _errorPassword = value; OnPropertyChanged(); } }
		private string _errorRePassword;
		public string errorRePassword { get => _errorRePassword; set { _errorRePassword = value; OnPropertyChanged(); } }
		private string _errorOTP;
		public string errorOTP { get => _errorOTP; set { _errorOTP = value; OnPropertyChanged(); } }
		public RegisterViewModel registerViewModel { get; set; }

		private object _currentView;
		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(); }
		}

		public AuthenViewModel()
		{
            CurrentView = new Login();

			registerViewModel = new RegisterViewModel();

			AuthenCommand = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) =>
			{
				Authentication(p);
				
			});

			PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
			RePasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { RePassword = p.Password; });

			RegisterViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				Email = string.Empty;
				errorPassword = string.Empty;
				errorEmail = string.Empty;
				errorStudentCode = string.Empty;
				errorRePassword = string.Empty;
				CurrentView = registerViewModel;
			});
			RegisterCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				Register();
			});
			LoginViewCommand = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) =>
			{
				CurrentView = new Login();
			});
			SubmitOTPCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				VerifyOTP();

			});
			ResendOTPCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				ResendOTP();
			});
			ForgotPasswordCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				CurrentView = new ForgotPasswordForm();
			});
			RequestPasswordResetCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				ForgotPassword();
			});

		}
		private async void ForgotPassword()
		{
			if (string.IsNullOrEmpty(Email))
			{
				errorEmail = "Email không được để trống !";
				return;
			}
			errorEmail = "";
			try
			{
				using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
				{
					var isExistUser = _context.Users.SingleOrDefault(u => u.Email == Email);
					if (isExistUser !=null)
					{
						var em = new EmailSender.EmailSender();
						await em.SendEmail(Email.Trim(), "Library Management Reset Password", "resetPasswordCode");
						CurrentView = new Login();
					}
					else
					{
						MessageBox.Show("Email bạn đã nhập không tồn tại trong hệ thống. Vui lòng kiểm tra lại.");
					}
				}
			}catch(Exception ex)
			{
				MessageBox.Show("Có lỗi xảy ra khi gửi mật khẩu mới. Vui lòng thử lại sau.");
			}
		}
		private async void ResendOTP()
		{
			var em = new EmailSender.EmailSender();
			await em.SendEmail(Email.Trim(), "Library Management Re-send OTP Code", "sendOTPCode");
			OtpCountdown();
		}

		private void DeleteExpiredOtps()
		{
			try
			{
				using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
				{
					var now = DateTime.UtcNow.ToLocalTime();
					var expiredOtps = _context.Users
						.Where(x => x.ExpirationCode.HasValue && x.ExpirationCode.Value < now)
						.ToList();

					foreach (var user in expiredOtps)
					{
						user.VerifyCode = null;
						user.ExpirationCode = null;
						_context.Update(user);
					}

					_context.SaveChanges();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}

		}

		private void VerifyOTP()
		{
			DeleteExpiredOtps();
			if (string.IsNullOrEmpty(OTP))
			{
				errorOTP = "Hãy nhập mã OTP để xác thực !";
				return;
			}
			else if (!Regex.IsMatch(OTP, @"\d"))
			{
				errorOTP = "OTP chỉ được phép chứa số !";
				return;
			}
			errorOTP = "";
			try
			{
				using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
				{
					var userToGrantAuthorize = _context.Users.SingleOrDefault(u => u.VerifyCode == OTP.Trim());

					if (userToGrantAuthorize != null)
					{
						userToGrantAuthorize.Role = "STUDENT";
						userToGrantAuthorize.Status = 1;
						userToGrantAuthorize.VerifyCode = null;
						userToGrantAuthorize.ExpirationCode = null;
						_context.Update(userToGrantAuthorize);
						_context.SaveChanges();
						CurrentView = new Login();
						MessageBox.Show("Xác thực thành công !");
					}
					else
					{
						OTP = "";
						MessageBox.Show("OTP không đúng hoặc đã hết hạn, vui lòng nhập lại !");
					}
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}

		}
		public static string Encrypt(string input)
		{
			MD5 md5 = MD5.Create();

			byte[] passwordBytes = Encoding.ASCII.GetBytes(input);
			byte[] hashedBytes = md5.ComputeHash(passwordBytes);

			StringBuilder sb = new StringBuilder();
			foreach (byte b in hashedBytes)
			{
				sb.Append(b.ToString("X2"));
			}
			return sb.ToString();
		}
		public bool IsValidEmail(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		private async void Register()
		{
			
			if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(RePassword) && string.IsNullOrEmpty(StudentCode))
			{
				errorEmail = "Email không được để trống !";
				errorPassword = "Password không được để trống !";
				errorStudentCode = "Student code không được để trống !";
				errorRePassword = "Re-Password không được để trống !";
				return;
			}
			else if (string.IsNullOrEmpty(Email))
			{
				errorEmail = "Email không được để trống !";
				errorPassword = "";
				errorStudentCode = "";
				errorRePassword = "";
				return;
			}
			else if (!IsValidEmail(Email))
			{
				errorEmail = "Email định dạng không hợp lệ !";
				errorPassword = "";
				errorStudentCode = "";
				errorRePassword = "";
				return;
			}
			else if (string.IsNullOrEmpty(StudentCode))
			{
				errorStudentCode = "Student code không được để trống !";
				errorEmail = "";
				errorPassword = "";
				errorRePassword = "";
				return;
			}
			else if (string.IsNullOrEmpty(Password))
			{
				errorPassword = "Password không được để trống !";
				errorEmail = "";
				errorStudentCode = "";
				errorRePassword = "";
				errorEmail = "";
				return;
			}
			else if (!Regex.IsMatch(Password, @"^(?=.+[A-Za-z])(?=.+\d)(?=.+[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$"))
			{
				errorPassword = "ít nhất 6 ký tự (1 chữ số, 1 ký tự đặc biệt)!";
				errorEmail = "";
				errorStudentCode = "";
				errorRePassword = "";
				errorEmail = "";
				return;
			}
			else if (string.IsNullOrEmpty(RePassword))
			{
				errorRePassword = "Re-Password không được để trống !";
				errorEmail = "";
				errorPassword = "";
				errorStudentCode = "";
				return;
			}
			else if (!Password.Trim().Equals(RePassword.Trim(), StringComparison.Ordinal))
			{
				errorRePassword = "Mật khẩu không trùng với mã nhập lại !";
				errorPassword = "";
				errorEmail = "";
				errorStudentCode = "";
				return;
			}
			else
			{
				errorPassword = "";
				errorEmail = "";
				errorStudentCode = "";
				errorRePassword = "";
			}
			try
			{
				using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
				{
					var isExistAccount = _context.Users.FirstOrDefault(a => a.Email.Equals(Email.Trim()));
					if (isExistAccount == null)
					{

						User user = new User
						{
							Email = Email.Trim(),
							StudentCode = StudentCode.Trim(),
							Password = Encrypt(Password.Trim()),
						};
						;
						user.Status = 0;

						_context.Add(user);
						_context.SaveChanges();

						var em = new EmailSender.EmailSender();
						await em.SendEmail(Email.Trim(), "Library Management OTP Code", "sendOTPCode");

						CurrentView = new OTPForm();
						OtpCountdown();
					}
					else
					{
						errorRePassword = "Tài khoản đã tồn tại !";
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi thực hiện đăng ký !");
			}
		}

		public void OtpCountdown()
		{
			_timeLeft = TimeSpan.FromMinutes(2);
			Countdown = $"OTP hết hạn trong: {_timeLeft:mm\\:ss}";

			_timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1)
			};
			_timer.Tick += Timer_Tick;
			_timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			_timeLeft = _timeLeft.Add(TimeSpan.FromSeconds(-1));

			Countdown = $"OTP hết hạn trong: {_timeLeft:mm\\:ss}";

			if (_timeLeft.TotalSeconds <= 0)
			{
				_timer.Stop();
				Countdown = "OTP đã hết hạn !";
			}
		}

		private void Authentication(System.Windows.Window p)
		{
			
			if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password))
			{
				errorEmail = "Email không được để trống !";
				errorPassword = "Password không được để trống !";
				return;
			}
			else if (string.IsNullOrEmpty(Email))
			{
				errorEmail = "Email không được để trống !";
				errorPassword = "";
				return;
			}
			else if (string.IsNullOrEmpty(Password))
			{
				errorEmail = "";
				errorPassword = "Password không được để trống !";
				return;
			}
			else
			{
				errorPassword = "";
				errorEmail = "";
			}
			using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
			{
				string passwordEncrypt = Encrypt(Password.Trim());
				var isExistAccount = _context.Users.FirstOrDefault(a => a.Email.Equals(Email.Trim()) && a.Password.Equals(passwordEncrypt));
				if (isExistAccount != null)
				{
					if (isExistAccount.Status != 1)
					{
						MessageBox.Show("Tài khoản của bạn chưa được kích hoạt !");
						return;
					}
					else if (isExistAccount.Role == "ADMIN")
					{
						SessionManager.CurrentUser = isExistAccount;

						AdminMainWindow adminMainWindow = new AdminMainWindow();
					/*	adminMainWindow.DataContext = new Admin.MainViewModel();*/
						adminMainWindow.Show();
						p.Close();
					}
					else if (isExistAccount.Role == "STUDENT")
					{
						SessionManager.CurrentUser = isExistAccount;

						StudentMainWindow studentMainWindow = new StudentMainWindow();
						studentMainWindow.DataContext = new Student.MainViewModel();
						studentMainWindow.Show();
						p.Close();
					}
					else if (isExistAccount.Role == "LIBRARIAN")
					{
						SessionManager.CurrentUser = isExistAccount;

						LibMainWindow libMainWindow = new LibMainWindow();
						libMainWindow.DataContext = new Librarian.MainViewModel();
						libMainWindow.Show();
						p.Close();
					} else
					{
                        MessageBox.Show("Invalid Role!");
                    }
				}
				else
				{
					errorPassword = "Email hoặc password không đúng !";
					return;
				}
			}
		}
	}
	public class RegisterViewModel
	{
	}

}
