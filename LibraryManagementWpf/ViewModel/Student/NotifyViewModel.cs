using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Student;
using Microsoft.EntityFrameworkCore;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;



namespace LibraryManagementWpf.ViewModel.Student
{
    public class NotifyViewModel : BaseViewModel
	{
		private string _searchText; 
		public string SearchText
		{
			get => _searchText;
			set
			{
				_searchText = value;
				OnPropertyChanged(nameof(SearchText));
			}
		}

		public IWebView2 WebView { get; set; }

		private ObservableCollection<Post> _ListPosts;
		public ObservableCollection<Post> ListPosts { get => _ListPosts; set { _ListPosts = value; OnPropertyChanged(nameof(ListPosts)); } } 

		public ICommand ViewPostDetailCommand { get; set; }
		public ICommand SearchPostCommand { get; set; }

		public NotifyViewModel()
		{
			LoadPosts();

			ViewPostDetailCommand = new RelayCommand<Post>((p) => { return p != null; }, async (p) =>
			{
				await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('first').setAttribute('style', 'display:none');");
				await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('second').setAttribute('style', 'display:none');");

				await WebView.CoreWebView2.ExecuteScriptAsync($"document.getElementById('title').innerHTML = '{p.Title}';");
				await WebView.CoreWebView2.ExecuteScriptAsync($"document.getElementById('date').innerHTML = '{p.CreatedAt}';");
				await WebView.CoreWebView2.ExecuteScriptAsync($"document.getElementById('content').innerHTML = '{p.Content}';");
			});

			SearchPostCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
				{
					if(!string.IsNullOrEmpty(SearchText))
					{
					ListPosts = new ObservableCollection<Post>(_context.Posts.Where(p => p.Title.Contains(SearchText)));
				}
					else
					{
						ListPosts = new ObservableCollection<Post>(_context.Posts.OrderByDescending(p => p.CreatedAt));
					}
				}
			});
		}

		public void LoadPosts()
		{
			using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
			{
				ListPosts = new ObservableCollection<Post>(_context.Posts
						   .AsNoTracking()
						   .OrderByDescending(p => p.CreatedAt));
			}
		}
	}

}
