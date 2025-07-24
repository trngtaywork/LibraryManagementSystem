using LibraryManagementWpf.Models;
using LibraryManagementWpf.ViewModel.Student;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WebView2.DOM;

namespace LibraryManagementWpf.View.Student
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : UserControl
    {
        public Notification()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await webview.EnsureCoreWebView2Async();
            await WebView2DOM.InitAsync(webview);
            string uri = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\View\Student\PostDetailsView.html";
            webview.CoreWebView2.Navigate(uri);
            if (DataContext is NotifyViewModel notiViewModel)
            {
                notiViewModel.WebView = webview;
            }

			if (DataContext is NotifyViewModel notifyViewModel)
		    {
				notifyViewModel.LoadPosts();
            }
        }


	}
}
