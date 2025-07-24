using Firebase.Storage;
using LibraryManagementWpf.Models;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace LibraryManagementWpf.ViewModel.Admin
{
	public class DocumentaryViewModel : BaseViewModel
	{
		private ObservableCollection<Models.Thesis> _Theses;
		public ObservableCollection<Models.Thesis> Theses { get => _Theses; set { _Theses = value; OnPropertyChanged(); } }
		public ObservableCollection<Models.Thesis> DisplayedListPerPage { get; private set; }

		private string _Id;
		public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }
		private string _Title;
		public string Title { get => _Title; set { _Title = value; OnPropertyChanged(); } }
		private string _Author;
		public string Author { get => _Author; set { _Author = value; OnPropertyChanged(); } }
		private string _DocUrl;
		public string DocUrl { get => _DocUrl; set { _DocUrl = value; OnPropertyChanged(); } }

		private string _FileDoc;
		public string FileDoc { get => _FileDoc; set { _FileDoc = value; OnPropertyChanged(); } }
		private string _Keyword;
		public string Keyword { get => _Keyword; set { _Keyword = value; OnPropertyChanged(); } }
		private string _TitleError;
		public string TitleError { get => _TitleError; set { _TitleError = value; OnPropertyChanged(); } }
		private string _AuthorError;
		public string AuthorError { get => _AuthorError; set { _AuthorError = value; OnPropertyChanged(); } }
		private string _SelectFileError;
		public string SelectFileError { get => _SelectFileError; set { _SelectFileError = value; OnPropertyChanged(); } }
		private string _selectedFilterOption = "All";
		public string SelectedFilterOption
		{
			get => _selectedFilterOption;
			set
			{
				if (_selectedFilterOption != value)
				{
					_selectedFilterOption = value;
					OnPropertyChanged(nameof(SelectedFilterOption));
					FilterCommand.Execute(null);
				}
			}
		}
		private string _DisplayTotalPage;
		public string DisplayTotalPage { get => _DisplayTotalPage; set { _DisplayTotalPage = value; OnPropertyChanged(); } }
		public ICommand SaveCommand { get; set; }
		public ICommand SelectFileCommand { get; set; }
		public ICommand EditCommand { get; set; }
		public ICommand RemoveCommand { get; set; }
		public ICommand SearchCommand { get; set; }
		public ICommand FilterCommand { get; set; }
		public ICommand NextPageCommand { get; }
		public ICommand PreviousPageCommand { get; }

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
					LoadDataThesisPagination();
			}
		}
		}
		public int TotalPages
		{
			get => (int)Math.Ceiling((double)Theses.Count / _itemPerPage);
		}

		public void LoadDataThesis()
			{
			using(var _context = new LibraryManageSystemContext())
				{
				Theses = new ObservableCollection<Thesis>(_context.Theses);
				}
			}
		public void LoadDataThesisPagination()
			{
			var skipItems = (CurrentPage - 1) * _itemPerPage;
			DisplayedListPerPage = new ObservableCollection<Models.Thesis>(Theses.Skip(skipItems).Take(_itemPerPage));
			OnPropertyChanged(nameof(DisplayedListPerPage));
			DisplayTotalPage = $"Showing {skipItems} to {skipItems + _itemPerPage} of {TotalPages} entries";
			Title = string.Empty;
			Author = string.Empty;
			DocUrl = string.Empty;
		}
		public bool IsPreviousPageEnabled;
		public bool IsNextPageEnabled;
		

		public DocumentaryViewModel() 
		{
			LoadDataThesis();

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

			LoadDataThesisPagination();

			SelectFileCommand = new RelayCommand<object>(p => true, async (p) =>
			{
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
					Filter = "Document Files|*.doc;*.docx;*.xls;*.xlsx",
					Title = "Chọn tệp tài liệu để tải lên"
				};

				if (openFileDialog.ShowDialog() == true)
				{
					FileDoc = openFileDialog.FileName; 
				}
			});
			FilterCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				using (var _context = new LibraryManageSystemContext())
				{
					IQueryable<Models.Thesis> query = _context.Theses;

					query = SelectedFilterOption switch
					{
						"Cũ nhất" => query.OrderByDescending(t => t.CreateAt),
						"Mới nhất" => query.OrderBy(t => t.CreateAt),
						"All" => query
					};

					Theses = new ObservableCollection<Models.Thesis>(query.ToList());
					LoadDataThesisPagination();
				}
			});
			SearchCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				using (var _context = new LibraryManageSystemContext())
				{
					Theses = new ObservableCollection<Models.Thesis>(_context.Theses.Where(t => t.Title.Contains(Keyword) || t.Author.Contains(Keyword)));
					CurrentPage = 1;
					LoadDataThesisPagination();
				}
			});
			SaveCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
			{
				try
				{
					if (string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Author) && string.IsNullOrEmpty(FileDoc))
					{
						TitleError = "Tiêu đề không được để trống."; 
						AuthorError = "Người thực hiện không được để trống.";
						SelectFileError = "File tài liệu không được để trống.";
						return;
					}
					else if (string.IsNullOrEmpty(Title))
					{
						TitleError = "Tiêu đề không được để trống.";
						return;
					}
					else if (string.IsNullOrEmpty(Author))
					{
						AuthorError = "Người thực hiện không được để trống.";
						return;
					}
					else if(string.IsNullOrEmpty(FileDoc))
					{
						SelectFileError = "File tài liệu không được để trống.";
						return;
					}
					
					SelectFileError = "";
					AuthorError = "";
					TitleError = "";

					if (FileDoc != null)
					{
						await StoreToFireBase();
					}
					using (var _context = new LibraryManageSystemContext())
					{
						if (!string.IsNullOrEmpty(Id))
						{
							var oldThesis = _context.Theses.FirstOrDefault(t => t.Id == int.Parse(Id));
							oldThesis.Title = Title;
							oldThesis.Author = Author;
							if(DocUrl != null)
							{
								oldThesis.FileDoc = DocUrl;
							}
							_context.Theses.Update(oldThesis);
						}
						else
						{
							Thesis thesis = new Thesis
							{
								Title = Title,
								Author = Author,
								FileDoc = DocUrl,
								CreateAt = DateTime.Now
							};
							_context.Theses.Add(thesis);
						}
						_context.SaveChanges();
						System.Windows.MessageBox.Show("Lưu thành công.");
						LoadDataThesis();
						LoadDataThesisPagination();
					}
				}
				catch { System.Windows.MessageBox.Show("Có lỗi khi lưu tài liệu."); }
			});
			EditCommand = new RelayCommand<Thesis>((p) => { return true; }, (p) =>
			{
				Id = p.Id.ToString();
				Title = p.Title;
				Author = p.Author;
			});
			RemoveCommand = new RelayCommand<Thesis>((p) =>
			{
				return p != null;
			}, (p) =>
			{
				var result = System.Windows.MessageBox.Show(
					$"Are you sure you want to delete the thesis title '{p.Title}' ?",
					"Delete Thesis",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question
				);
				if (result == MessageBoxResult.Yes)
				{
					using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
					{
						try
						{
							var thesisInDb = _context.Theses.FirstOrDefault(t => t.Id == p.Id);
							if (thesisInDb != null)
							{
								_context.Theses.Remove(thesisInDb);
								_context.SaveChangesAsync();
							}
							System.Windows.MessageBox.Show("Xóa tài liệu thành công !");

							LoadDataThesis();
							LoadDataThesisPagination();
						}
						catch (Exception ex)
						{
                            System.Windows.MessageBox.Show("Xóa tài liệu không thành công !");
						}
					}
				}
			});
		}

		public async Task StoreToFireBase()
		{
			var firebaseStorage = new FirebaseStorage("fir-60e00.appspot.com");
		
			try
			{
				using (var stream = new FileStream(FileDoc, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					var task = await firebaseStorage
						.Child("documents")
						.Child(Path.GetFileName(FileDoc))
						.PutAsync(stream);
					DocUrl = task;
				}
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show("Lỗi khi tải tệp lên: " + ex.Message);
			}
		}
			
	}
}
