using LibraryManagementWpf.Models;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementWpf.ViewModel.Admin
{
	public class PostViewModel : BaseViewModel
	{
		public ICommand AddCommand { get; set; }
		public ICommand RemoveCommand { get; set; }
		public ICommand EditCommand { get; set; }
		public ICommand DoEditCommand {  get; set; }
		public IWebView2 WebView { get; set; }
		public ICommand NextPageCommand { get; }
		public ICommand PreviousPageCommand { get; }

		private ObservableCollection<Post> _ListPosts;
		public ObservableCollection<Post> ListPosts {  get => _ListPosts; set { _ListPosts = value; OnPropertyChanged();  }	}
		public ObservableCollection<Models.Post> DisplayedListPerPage { get; private set; }
		private string _DisplayTotalPage;
		public string DisplayTotalPage { get => _DisplayTotalPage; set { _DisplayTotalPage = value; OnPropertyChanged(); } }

		private int _currentPage = 1;
		private int _itemPerPage = 10;
		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				if (_currentPage != value)
				{
					_currentPage = value;
					OnPropertyChanged(nameof(CurrentPage));
					LoadDataPostPagination();
				}
			}
		}
		public int TotalPages
		{
			get => (int)Math.Ceiling((double)ListPosts.Count / _itemPerPage);
		}

		public void LoadDataPostPagination()
		{
			var skipItems = (CurrentPage - 1) * _itemPerPage;
			DisplayedListPerPage = new ObservableCollection<Models.Post>(ListPosts.Skip(skipItems).Take(_itemPerPage));
			OnPropertyChanged(nameof(DisplayedListPerPage));
			DisplayTotalPage = $"Showing {skipItems} to {skipItems + _itemPerPage} of {TotalPages} entries";
		}
		public bool IsPreviousPageEnabled;
		public bool IsNextPageEnabled;

		public PostViewModel()
		{

			using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
			{
				ListPosts = new ObservableCollection<Post>(_context.Posts);
			}

			NextPageCommand = new RelayCommand<object>(
				(_) => CurrentPage < TotalPages,
				(_) =>
				{
					CurrentPage = CurrentPage + 1; if (CurrentPage > 1) { IsPreviousPageEnabled = true; } else { IsPreviousPageEnabled = false; }
				});
			PreviousPageCommand = new RelayCommand<object>(
				(_) => CurrentPage > 1,
				(_) =>
				{
					CurrentPage = CurrentPage - 1; if (CurrentPage < TotalPages) { IsNextPageEnabled = true; } else { IsNextPageEnabled = false; }
				});

			LoadDataPostPagination();

			AddCommand = new RelayCommand<object>((p) => 
			{
				return true; 
			}, async (p) =>
			{
				var title = await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('post-title').value.trim()");
				var content = await WebView.CoreWebView2.ExecuteScriptAsync("contentEditor.getData()");
				if (string.IsNullOrEmpty(content?.Trim('\"')) && string.IsNullOrEmpty(title?.Trim('\"')))
				{
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorTitle').textContent = 'Không được để trống tiêu đề !';");
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorContent').textContent = 'Không được để trống nội dung !';");
					return;
				}
				else if (string.IsNullOrEmpty(title?.Trim('\"')))
				{
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorTitle').textContent = 'Không được để trống tiêu đề !';");
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorContent').textContent = '';");
					return;
				}
				else if (string.IsNullOrEmpty(content?.Trim('\"')))
				{
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorTitle').textContent = '';");
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorContent').textContent = 'Không được để trống nội dung !';");
					return;
				}
				else
				{
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorTitle').textContent = '';");
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorContent').textContent = '';");
				}

				using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
				{
					try
					{
						var post = new Post()
						{
							Title = title?.Trim('\"'),
							Content = content?.Trim('\"')
						};
						_context.Add(post);
						_context.SaveChanges();

						var updatedPosts = _context.Posts.ToList();
						ListPosts.Clear();

						foreach (var updatedPost in updatedPosts)
						{
							ListPosts.Add(updatedPost); 
						}
						await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('post-title').value = '';");
						await WebView.CoreWebView2.ExecuteScriptAsync("contentEditor.setData('');");
						MessageBox.Show("Thêm mới bài viết thành công !");
						LoadDataPostPagination();
					}
					catch (Exception ex)
					{
						MessageBox.Show("Thêm mới bài viết không thành công !");
					}
				}
			});

			RemoveCommand = new RelayCommand<Post>((p) =>
			{
				return p!=null;
			}, (p) =>
			{
				var result = MessageBox.Show(
					$"Are you sure you want to delete the post title '{p.Title}' ?",
					"Delete Post",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question
				);
				if (result == MessageBoxResult.Yes)
				{
					using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
					{
						try
						{
							var postInDb = _context.Posts.FirstOrDefault(a => a.Id == p.Id);
							if (postInDb != null)
							{
								_context.Posts.Remove(postInDb);
								_context.SaveChangesAsync();
							}
							MessageBox.Show("Xóa bài viết thành công !");

							ListPosts = new ObservableCollection<Post>(_context.Posts);
							LoadDataPostPagination();
						}
						catch (Exception ex)
						{
							MessageBox.Show("Xóa bài viết không thành công !");
						}
					}
				}
			});


			EditCommand = new RelayCommand<Post>((p) =>
			{
				return p!=null;
			}, async (p) =>
			{
				await WebView.CoreWebView2.ExecuteScriptAsync($"document.getElementById('post-id').value = '{p.Id}';");
				await WebView.CoreWebView2.ExecuteScriptAsync($"document.getElementById('post-title').value = '{p.Title}';");
				await WebView.CoreWebView2.ExecuteScriptAsync($"contentEditor.setData('{p.Content}');");	
			});

			DoEditCommand = new RelayCommand<Post>((p) =>
			{
				return true;
			}, async (p) =>
			{
				var idStr = await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('post-id').value.trim()");
				var title = await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('post-title').value.trim()");
				var content = await WebView.CoreWebView2.ExecuteScriptAsync("contentEditor.getData()");
				if (string.IsNullOrEmpty(idStr?.Trim('\"')) && string.IsNullOrEmpty(idStr?.Trim('\"')))
				{
					MessageBox.Show("Vui lòng chọn bài đăng trước khi chỉnh sửa bài viết !");
					return;
				}
				else if (string.IsNullOrEmpty(content?.Trim('\"')) && string.IsNullOrEmpty(title?.Trim('\"')))
				{
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorTitle').textContent = 'Không được để trống tiêu đề !';");
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorContent').textContent = 'Không được để trống nội dung !';");
					return;
				}
				else if (string.IsNullOrEmpty(title?.Trim('\"')))
				{
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorTitle').textContent = 'Không được để trống tiêu đề !';");
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorContent').textContent = '';");
					return;
				}
				else if (string.IsNullOrEmpty(content?.Trim('\"')))
				{
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorTitle').textContent = '';");
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorContent').textContent = 'Không được để trống nội dung !';");
					return;
				}
				else
				{
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorTitle').textContent = '';");
					await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('errorContent').textContent = '';");
				}
				using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
				{
					try
					{
						int id = int.Parse(idStr?.Trim('\"'));
						var postToUpdate = _context.Posts.SingleOrDefault(p => p.Id == id);
						postToUpdate.Title = title.Trim('\"');
						postToUpdate.Content = content.Trim('\"');
						_context.Update(postToUpdate);
						_context.SaveChanges();

						ListPosts = new ObservableCollection<Post>(_context.Posts);

						await WebView.CoreWebView2.ExecuteScriptAsync("document.getElementById('post-title').value = '';");
						await WebView.CoreWebView2.ExecuteScriptAsync("contentEditor.setData('');");
						MessageBox.Show("Cập nhật bài viết thành công !");
						LoadDataPostPagination();
					}
					catch (Exception ex)
					{
						MessageBox.Show("Cập nhật bài viết không thành công !");
					}
				}
			});
		}
	}
}
