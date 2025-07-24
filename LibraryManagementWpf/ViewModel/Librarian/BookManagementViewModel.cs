using LibraryManagementWpf.DTO;
using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Librarian;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Firebase.Storage;
using LibraryManagementWpf.View.Admin;
using LibraryManagementWpf.ViewModel.Admin;
using Microsoft.VisualBasic.ApplicationServices;
using LibraryManagementWpf.ViewModel.Common;


namespace LibraryManagementWpf.ViewModel.Librarian
{
    public class BookManagementViewModel : BaseViewModel
    {

		private Models.User _currentUser;
		public Models.User CurrentUser
		{
			get => _currentUser;
			set
			{
				_currentUser = value;
				OnPropertyChanged(nameof(CurrentUser));
			}
		}
		private string _BookId;
		public string BookId { get => _BookId; set { _BookId = value; OnPropertyChanged(); } }
		private string _Title;
		public string Title { get => _Title; set { _Title = value; OnPropertyChanged(); } }
		private string _Author;
		public string Author { get => _Author; set { _Author = value; OnPropertyChanged(); } }
		private string _Quantity;
		public string Quantity { get => _Quantity; set { _Quantity = value; OnPropertyChanged(); } }
		private DateTime? _publicDate;
		public DateTime? PublicDate
		{
			get => _publicDate;
			set
			{
				_publicDate = value;
				OnPropertyChanged(nameof(PublicDate));
			}
		}

		private BitmapImage _selectedImage;
		public BitmapImage SelectedImage
		{
			get => _selectedImage;
			set
			{
				_selectedImage = value;
				OnPropertyChanged(nameof(SelectedImage));
			}
		}
		private string _ImageUrl;
		public string ImageUrl { get => _ImageUrl; set { _ImageUrl = value; OnPropertyChanged(); } }
		private string _CategoryId;
		public string CategoryId { get => _CategoryId; set { _CategoryId = value; OnPropertyChanged(); } }
		private string _TitleError;
		public string TitleError { get => _TitleError; set { _TitleError = value; OnPropertyChanged(); } }
		private string _AuthorError;
		public string AuthorError { get => _AuthorError; set { _AuthorError = value; OnPropertyChanged(); } }
		private string _QuantityError;
		public string QuantityError { get => _QuantityError; set { _QuantityError = value; OnPropertyChanged(); } }
		private string _PublicDateError;
		public string PublicDateError { get => _PublicDateError; set { _PublicDateError = value; OnPropertyChanged(); } }
		private string _ImageError;
		public string ImageError { get => _ImageError; set { _ImageError = value; OnPropertyChanged(); } }
		private string _CategoryError;
		public string CategoryError { get => _CategoryError; set { _CategoryError = value; OnPropertyChanged(); } }
        public ICommand AddNewBookViewCommand { get; set; }
		public ICommand DoAddNewBookCommand { get; set; }

		public ICommand EditBookViewCommand { get; set; }
		public ICommand DoEditBookCommand { get; set; }
		public ICommand SelectImageCommand {  get; set; }
		public ICommand SelectImageCommand1 { get; set; }
		public ICommand CancelCommand {  get; set; }
		private ObservableCollection<BookDTO> _Books;
		public ObservableCollection<BookDTO> Books { get => _Books; set { _Books = value; OnPropertyChanged(); } }
        private int bookCount;
        public int BookCount
        {
            get => bookCount;
            private set
            {
                bookCount = value;
                OnPropertyChanged();
            }
        }
		private ObservableCollection<Category> _ListCategory;
		public ObservableCollection<Category> ListCategory { get => _ListCategory; set { _ListCategory = value; OnPropertyChanged(); } }

        public ObservableCollection<Category> Categories { get; set; }
        private object selectedCategory;
        public object SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged();
                LoadBooks();
            }
        }

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                LoadBooks();
            }
        }

        public ObservableCollection<string> SortOptions { get; set; }
        private string selectedSortOption;
        public string SelectedSortOption
        {
            get => selectedSortOption;
            set
            {
                selectedSortOption = value;
                OnPropertyChanged();
                LoadBooks();
            }
        }

        public BookManagementViewModel()
        {
			var currentUserSession = SessionManager.CurrentUser;

			if (currentUserSession != null)
			{
				CurrentUser = currentUserSession;
			}
            Books = new ObservableCollection<BookDTO>();
            Categories = new ObservableCollection<Category>();
            SortOptions = new ObservableCollection<string>
            {
                "Oldest Book",
                "Newest Book",
                "Title",
                "Publication Date (Ascending)",
                "Publication Date (Descending)"
            };
            LoadBooks();
            LoadCategories();

            SelectedCategory = Categories.FirstOrDefault(c => c.CategoryName == "All");
            SelectedSortOption = SortOptions.First();
			CancelCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
			{
				Title = "";
				Author = "";
				Quantity = "";
				PublicDate = null;
				SelectedImage = null;
				ImageUrl = "";
				CategoryId = "";
				BookId = "";
				p.Close();
			});
			AddNewBookViewCommand = new RelayCommand<object>(p => true, p => OpenAddNewBookDialog());
			SelectImageCommand = new RelayCommand<object>(p => true, async (p) =>
			{
				OpenFileDialog openFileDialog = new OpenFileDialog
				{
					Filter = "Image files (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg",
					Title = "Select an Image File"
				};

				if (openFileDialog.ShowDialog() == true)
				{
					SelectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
				}
			});
			SelectImageCommand1 = new RelayCommand<EditBookParameter>(p => true, async (p) =>
			{
				OpenFileDialog openFileDialog = new OpenFileDialog
				{
					Filter = "Image files (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg",
					Title = "Select an Image File"
				};

				if (openFileDialog.ShowDialog() == true)
				{
					SelectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
					if (SelectedImage != null)
					{
						var filePath = SelectedImage.UriSource.LocalPath;
						var firebaseStorage = new FirebaseStorage("fir-60e00.appspot.com");

						try
						{
							using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
							{
								var task = await firebaseStorage
									.Child("images")
									.Child(System.IO.Path.GetFileName(filePath))
									.PutAsync(stream);

								ImageUrl = task;
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message);
						}
					}
					try
					{
						using (var _context = new LibraryManageSystemContext())
						{
							Book book = _context.Books.SingleOrDefault(x => x.BookId == int.Parse(p.BookId));
							book.Image = ImageUrl;
							_context.Update(book);
							_context.SaveChanges();
						}
					}
					catch(Exception ex)
					{
						MessageBox.Show("Lỗi khi lưu ảnh: " + ex.Message);
					}
					
				}
			});
			DoAddNewBookCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
			{
				AddNewBook(p);
			});
			EditBookViewCommand = new RelayCommand<BookDTO>((p) => { return true; }, (p) => {
				var editBook = new EditBook();
				var viewModel = new EditBookViewModel(p); 
				editBook.DataContext = viewModel;
				editBook.ShowDialog();
			});


			DoEditBookCommand = new RelayCommand<EditBookParameter>((p) => true, (p) =>
			{
				if (p != null)
				{
					BookId = p.BookId;
					Title = p.Title;
					Author = p.AuthorNames;
					Quantity = p.Quantity;
					PublicDate = p.PublicationDate;
					SelectedImage = p.SelectedImage;
					CategoryId = p.CategoryId;
					var editBook = Application.Current.Windows.OfType<EditBook>().FirstOrDefault();
					ValidateAndEdit(editBook);
				}
			});
		}

		public async void ValidateAndEdit(EditBook editBook)
		{
			if (string.IsNullOrWhiteSpace(Title))
			{
				MessageBox.Show("Tiêu đề không được để trống.");
				return;
			}

			if (string.IsNullOrWhiteSpace(Author))
			{
				MessageBox.Show("Tên tác giả không được để trống.");
				return;
				
			}

			if (string.IsNullOrWhiteSpace(Quantity) || !int.TryParse(Quantity, out int quantity) || quantity < 0)
			{
				MessageBox.Show("Số lượng không được để trống (>=0 và là chữ số).");
				return;
				
			}

			if (!PublicDate.HasValue)
			{
				MessageBox.Show("Ngày phát hành không được để trống.");
				return;
			
			}

			if (PublicDate.Value.Date > DateTime.Now.Date)
			{
				MessageBox.Show("Ngày phát hành phải trước hoặc là ngày hiện tại.");
				return;
		
			}

			if (string.IsNullOrEmpty(CategoryId))
			{
				MessageBox.Show("Hãy chọn thể loại.");
				return;
				
			}

			var authorNames = Author.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
											 .Select(a => a.Trim())
											 .Where(a => !string.IsNullOrWhiteSpace(a))
											 .ToList();

			if (authorNames.Count == 0)
			{
				MessageBox.Show("Tên tác giả không được để trống.");
				return;
			}

			try
			{
				using (var db = new LibraryManageSystemContext())
				{
					var authors = new List<Author>();

					var bookToUpdate = db.Books
						.Include(b => b.Authors)
						.SingleOrDefault(b => b.BookId == int.Parse(BookId));

					foreach (var authorName in authorNames)
					{
						var existingAuthor = db.Authors
							.FirstOrDefault(a => a.AuthorName.ToLower() == authorName.ToLower());

						if (existingAuthor != null)
						{
							if (bookToUpdate.Authors.Any(a => a.AuthorId == existingAuthor.AuthorId))
							{
								continue;
							}
							authors.Add(existingAuthor);
						}
						else
						{
							var newAuthor = new Author
							{
								AuthorName = authorName
							};

							db.Authors.Add(newAuthor);
							authors.Add(newAuthor);
						}
					}

					bookToUpdate.Title = Title.Trim();
					bookToUpdate.Quantity = quantity;
					bookToUpdate.PublicationDate = PublicDate.Value;
					bookToUpdate.CategoryId = int.Parse(CategoryId.Trim());

					foreach (var author in authors)
					{
						bookToUpdate.Authors.Add(author);
					}

					db.SaveChanges();
					MessageBox.Show("Cập nhật thành công.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
					LoadBooks();
					editBook.Close();
					Title = null;
					Author = null;
					Quantity = null;
					PublicDate = null;
					SelectedImage = null;
					ImageUrl = null;
					CategoryId = null;
					BookId = null;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		public async void AddNewBook(Window p)
		{
			if (string.IsNullOrWhiteSpace(Title))
			{
				TitleError="Tiêu đề không được để trống.";
				AuthorError = "";
				QuantityError = "";
				PublicDateError = "";
				CategoryError = "";
				ImageError = "";
				return;
			}

			if (string.IsNullOrWhiteSpace(Author))
			{
				AuthorError = "Tên tác giả không được để trống.";
				TitleError = "";
				QuantityError = "";
				PublicDateError = "";
				CategoryError = "";
				ImageError = "";
				return;
			}

			if (string.IsNullOrWhiteSpace(Quantity) || !int.TryParse(Quantity, out int quantity) || quantity < 0)
			{
				QuantityError = "Số lượng không được để trống (>=0 và là chữ số).";
				AuthorError = "";
				TitleError = "";
				PublicDateError = "";
				CategoryError = "";
				ImageError = "";
				return;
			}

			if (!PublicDate.HasValue)
			{
				AuthorError = "";
				TitleError = "";
				QuantityError = "";
				CategoryError = "";
				ImageError = "";
				PublicDateError = "Ngày phát hành không được để trống.";
				return;
			}

			if (PublicDate.Value.Date > DateTime.Now.Date)
			{
				AuthorError = "";
				TitleError = "";
				QuantityError = "";
				CategoryError = "";
				ImageError = "";
				PublicDateError = "Ngày phát hành phải trước hoặc là ngày hiện tại.";
				return;
			}

			if (string.IsNullOrEmpty(CategoryId))
			{
				AuthorError = "";
				TitleError = "";
				QuantityError = "";
				PublicDateError = "";
				ImageError = "";
				CategoryError = "Hãy chọn thể loại.";
				return;
			}

			if (SelectedImage != null)
			{
				var filePath = SelectedImage.UriSource.LocalPath;
				var firebaseStorage = new FirebaseStorage("fir-60e00.appspot.com");

				try
				{
					using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						var task = await firebaseStorage
							.Child("images")
							.Child(System.IO.Path.GetFileName(filePath))
							.PutAsync(stream);

						ImageUrl = task;
						MessageBox.Show("Tải ảnh lên thành công với đường dẫn: " + ImageUrl);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message);
				}
			}
			else
			{
				AuthorError = "";
				TitleError = "";
				QuantityError = "";
				PublicDateError = "";
				CategoryError = "";
				ImageError = "Hãy chọn ảnh.";
				return;
			}

			var authorNames = Author.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
											 .Select(a => a.Trim())
											 .Where(a => !string.IsNullOrWhiteSpace(a))
											 .ToList();

			if (authorNames.Count == 0)
			{
				TitleError = "";
				QuantityError = "";
				PublicDateError = "";
				CategoryError = "";
				ImageError = "";
				AuthorError = "Tên tác giả không được để trống.";
				return;
			}

			try
			{
				using (var db = new LibraryManageSystemContext())
				{
					var authors = new List<Author>();
					foreach (var authorName in authorNames)
					{
						var existingAuthor = db.Authors
							.FirstOrDefault(a => a.AuthorName.ToLower() == authorName.ToLower());

						if (existingAuthor != null)
						{
							authors.Add(existingAuthor);
						}
						else
						{
							var newAuthor = new Author
							{
								AuthorName = authorName
							};
							db.Authors.Add(newAuthor);
							authors.Add(newAuthor);
						}
					}

					db.SaveChanges();

					var newBook = new Book
					{
						Title = Title.Trim(),
						Quantity = quantity,
						PublicationDate = PublicDate.Value,
						Image = ImageUrl,
						CategoryId = int.Parse(CategoryId.Trim()),
						Authors = authors,
						CreatedBy = CurrentUser.Fullname,
						CreatedAt = DateTime.Now,
					};

					db.Books.Add(newBook);
					db.SaveChanges();
					LoadBooks();
					MessageBox.Show("Thêm mới sách thành công.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
					p.Close();
				}
				
				Title = "";
				Author = "";
				Quantity = "";
				PublicDate = null;
				SelectedImage = null;
				ImageUrl = "";
				CategoryId = null;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi khi thêm mới: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
        }

        public void LoadBooks()
        {
            using (var db = new LibraryManageSystemContext())
            {

				ListCategory = new ObservableCollection<Category>(db.Categories);

                var query = db.Books.Include(b => b.Authors).Include(b => b.Category).AsQueryable();

                if (!string.IsNullOrEmpty(SearchText))
                {
                    query = query.Where(b => b.Title.Contains(SearchText) ||
                                              b.Authors.Any(a => a.AuthorName.Contains(SearchText)));
                }

                if (SelectedCategory is Category selectedCategory && selectedCategory.CategoryName != "All")
                {
                    query = query.Where(b => b.CategoryId == selectedCategory.CategoryId);
                }

                switch (SelectedSortOption)
                {
                    case "Oldest Book":
                        query = query.OrderBy(b => b.CreatedAt);
                        break;
                    case "Newest Book":
                        query = query.OrderByDescending(b => b.CreatedAt);
                        break;
                    case "Title":
                        query = query.OrderBy(b => b.Title);
                        break;
                    case "Publication Date (Ascending)":
                        query = query.OrderBy(b => b.PublicationDate);
                        break;
                    case "Publication Date (Descending)":
                        query = query.OrderByDescending(b => b.PublicationDate);
                        break;
                    default:
                        break;
                }

                var booksFromDb = query.ToList();
                Books.Clear();
                foreach (var book in booksFromDb)
                {
                    var authorNames = book.Authors?.Select(a => a.AuthorName) ?? Enumerable.Empty<string>();
                    var categoryName = book.Category?.CategoryName ?? "Uncategorized";

                    Books.Add(new BookDTO
                    {
                        BookId = book.BookId,
                        Title = book.Title,
                        AuthorNames = string.Join(", ", authorNames),
						CategoryId = book.CategoryId,
                        CategoryName = categoryName,
                        Quantity = book.Quantity,
                        PublicationDate = book.PublicationDate,
                        CreatedAt = book.CreatedAt,
                        CreatedBy = book.CreatedBy,
                        Image = book.Image
                    });
                }
                BookCount = Books.Count;
            }
        }

        private void LoadCategories()
        {
            using (var context = new LibraryManageSystemContext())
            {
                var categoriesFromDb = context.Categories.ToList();
                Categories.Clear();
                Categories.Add(new Category { CategoryName = "All" });
                foreach (var category in categoriesFromDb)
                {
                    Categories.Add(category);
                }
            }
        }

        private void OpenAddNewBookDialog()
        {
			var addBookWindow = new AddNewBook();
            addBookWindow.ShowDialog();
        }

    }
}
