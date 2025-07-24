using LibraryManagementWpf.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryManagementWpf.View.Common
{
	/// <summary>
	/// Interaction logic for RegisterForm.xaml
	/// </summary>
	public partial class RegisterForm : UserControl
	{
		public RegisterForm()
		{
			InitializeComponent();
		}

		private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (txtPassword.Password.Length > 0)
			{
				placeHolder.Visibility = Visibility.Hidden;
			}
			else
			{
				placeHolder.Visibility = Visibility.Collapsed;
			}
		}

		
		private void txtRePassword_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (txtRePassword.Password.Length > 0)
			{
				placeHolder1.Visibility = Visibility.Hidden;
			}
			else
			{
				placeHolder1.Visibility = Visibility.Collapsed;
			}
		}

		private void btnShowPassword_Click(object sender, RoutedEventArgs e)
		{

			txtPassword.Visibility = Visibility.Hidden;
			txtPasswordVisible.Visibility = Visibility.Visible;
			txtPasswordVisible.Text = txtPassword.Password;

			txtRePassword.Visibility = Visibility.Hidden;
			txtPasswordVisible1.Visibility = Visibility.Visible;
			txtPasswordVisible1.Text = txtRePassword.Password;
		}
		private void btnShowPassword_MouseLeave(object sender, MouseEventArgs e)
		{
			txtPasswordVisible.Visibility = Visibility.Hidden;
			txtPassword.Visibility = Visibility.Visible;

			txtPasswordVisible1.Visibility = Visibility.Hidden;
			txtRePassword.Visibility = Visibility.Visible;
		}
	}
}
