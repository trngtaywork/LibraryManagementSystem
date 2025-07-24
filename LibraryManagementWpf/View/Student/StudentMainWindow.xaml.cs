using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace LibraryManagementWpf.View.Student
{
    /// <summary>
    /// Interaction logic for StudentMainWindow.xaml
    /// </summary>
    public partial class StudentMainWindow : Window
	{
        public StudentMainWindow()
        {
            InitializeComponent();
        }

		private bool isMenuVisible = true;
		private void MenuButton_Click(object sender, RoutedEventArgs e)
		{
			if (isMenuVisible)
			{
				MenuColumn.Width = new GridLength(0);
				btnDisplayMenu.Visibility = Visibility.Visible;
			}
			else
			{
				MenuColumn.Width = new GridLength(200);
				btnDisplayMenu.Visibility = Visibility.Hidden;

			}

			isMenuVisible = !isMenuVisible;
		}

        
        private async void TestSendEmailButton_Click(object sender, RoutedEventArgs e)
        {
            string toEmail = "thutranzzzzzz@gmail.com";
            string subject = "Test Email";
			string replaceContentType = "sendBill";
            var em = new EmailSender.EmailSender();
            MessageBox.Show("Sending email, please wait...");

            await em.SendEmail(toEmail, subject, replaceContentType);

            MessageBox.Show($"Email with a new password has been sent to {toEmail}");
        }
        
    }
}
