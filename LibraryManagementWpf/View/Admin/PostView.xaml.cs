using LibraryManagementWpf.ViewModel.Admin;
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
using WebView2.DOM;

namespace LibraryManagementWpf.View.Admin
{
	/// <summary>
	/// Interaction logic for Post.xaml
	/// </summary>
	public partial class PostView : UserControl
	{
		public PostView()
		{
			InitializeComponent();
		}
		private async void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			await webview.EnsureCoreWebView2Async();
			await WebView2DOM.InitAsync(webview);

			string uri = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\View\Admin\FormEditPost.html";
			webview.CoreWebView2.Navigate(uri);

			if (DataContext is PostViewModel viewModel)
			{
				viewModel.WebView = webview;
			}
		}
	}
}
