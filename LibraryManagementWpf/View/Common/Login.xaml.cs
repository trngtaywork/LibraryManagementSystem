using LibraryManagementWpf.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace LibraryManagementWpf.View.Common
{
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }


        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Length > 0)
            {
                placeholderText.Visibility = Visibility.Hidden;
            }
            else
            {
                placeholderText.Visibility = Visibility.Collapsed;
            }
        }

        private void btnShowPassword_Click(object sender, RoutedEventArgs e)
        {

            txtPassword.Visibility = Visibility.Hidden;
            txtPasswordVisible.Visibility = Visibility.Visible;
            txtPasswordVisible.Focus();
            txtPasswordVisible.Text = txtPassword.Password;
        }

        private void btnShowPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            txtPasswordVisible.Visibility = Visibility.Hidden;
            txtPassword.Visibility = Visibility.Visible;
        }

    }
}
