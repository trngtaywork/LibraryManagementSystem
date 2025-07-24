using Firebase.Storage;
using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Admin;
using LibraryManagementWpf.View.Student;
using LibraryManagementWpf.ViewModel.Admin;
using LibraryManagementWpf.ViewModel.Common;
using LibraryManagementWpf.ViewModel.Librarian;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static WebView2.DOM.HTMLInputElement;


namespace LibraryManagementWpf.ViewModel.Student
{
    public class MainViewModel : BaseViewModel
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
		private string _Email;
        public string Email
        {
            get => _Email;
            set { _Email = value; OnPropertyChanged(); }
        }

        private string _StudentCode;
        public string StudentCode
        {
            get => _StudentCode;
            set { _StudentCode = value; OnPropertyChanged(); }
        }

        public ICommand NotifyListCommand { get; private set; }
        public ICommand BookListCommand { get; private set; }
        public ICommand MyBooksCommand { get; private set; }
        public ICommand StudentProfileCommand { get; private set; }
        public ICommand StudentFinesCommand { get; private set; }
        public ICommand LoginViewCommand { get; private set; }
        public ICommand StudentFinesHistoryCommand { get; private set; }
        public ICommand ReportsCommand { get; private set; }

        public NotifyViewModel NotifyListVM { get; private set; }
        public BookListViewModel BookListVM { get; private set; }
        public MyBooksViewModel MyBooksVM { get; private set; }
        public StudentProfileViewModel StudentProfileVM { get; private set; }
        public StudentFinesViewModel StudentFinesVM { get; private set; }
        public StudentFinesHistoryViewModel StudentFinesHistoryVM { get; private set; }
        public ReportsViewModel ReportsVM { get; private set; }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }
        public void Init(MainViewModel mainViewModel)
        {
            NotifyListVM = new NotifyViewModel();
            BookListVM = new BookListViewModel(mainViewModel);
            MyBooksVM = new MyBooksViewModel(mainViewModel);
            StudentProfileVM = new StudentProfileViewModel(mainViewModel);
            StudentFinesVM = new StudentFinesViewModel();
            StudentFinesHistoryVM = new StudentFinesHistoryViewModel();
            ReportsVM = new ReportsViewModel();
        }
        public MainViewModel()
        {
            CurrentUser = SessionManager.CurrentUser;
			var currentUser = SessionManager.CurrentUser;
            if (currentUser != null)
            {
                Email = currentUser.Email;
                StudentCode = currentUser.StudentCode;
            }

            Init(this);

            CurrentView = StudentProfileVM;

            NotifyListCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
				Init(this);

				CurrentView = NotifyListVM;
            });
            BookListCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
				Init(this);

				BookList bl = new BookList();
                CurrentView = BookListVM;
            });
            MyBooksCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
				Init(this);

				MyBooks mb = new MyBooks();
                CurrentView = MyBooksVM;
            });
            StudentProfileCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
				Init(this);

				StudentProfile sp = new StudentProfile();
                CurrentView = StudentProfileVM;
            });
            StudentFinesCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
				Init(this);

				StudentFines sf = new StudentFines();
                CurrentView = StudentFinesVM;
            });
            StudentFinesHistoryCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
				Init(this);

				StudentFinesHistory sfh = new StudentFinesHistory();
                CurrentView = StudentFinesHistoryVM;
            });
            ReportsCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
				Init(this);

				ReportList r = new ReportList();
                CurrentView = ReportsVM;
            });
            LoginViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                SessionManager.ClearSession();
                p.Close();
            });
        }
    }

    public class BookListViewModel : BaseViewModel
    {
        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                LoadBooks();
            }
        }
        private string _searchTextTitle;
        public string SearchTextTitle
        {
            get => _searchTextTitle;
            set
            {
                _searchTextTitle = value;
                OnPropertyChanged();
                LoadBooks();
            }
        }
        private string _searchTextAuthor;
        public string SearchTextAuthor
        {
            get => _searchTextAuthor;
            set
            {
                _searchTextAuthor = value;
                OnPropertyChanged();
                LoadBooks();
            }
        }
        private int _totalPages;
        public int TotalPages { get => _totalPages; set { _totalPages = value; OnPropertyChanged(); } }
        private ObservableCollection<Book> _booksList;
        public ObservableCollection<Book> BooksList { get => _booksList; set { _booksList = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _categoryList;
        public ObservableCollection<string> CategoriesList { get => _categoryList; set { _categoryList = value; OnPropertyChanged(); } }

        private string _selectedSortCategory = "Default";
        public string SelectedSortCategory
        {
            get => _selectedSortCategory;
            set { _selectedSortCategory = value; OnPropertyChanged(); LoadBooks(); }
        }

        private string _selectedSortOrder = "Default";
        public string SelectedSortOrder
        {
            get => _selectedSortOrder;
            set { _selectedSortOrder = value; OnPropertyChanged(); LoadBooks(); }
        }

        public ICommand ReserveBookCommand { get; set; }
        public ICommand ViewDetailCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }

        public BookListViewModel(MainViewModel _studentMainViewModel)
        {
            ViewDetailCommand = new RelayCommand<int?>((bookId) => bookId != null && bookId > 0, (bookId) => ViewDetail(bookId.Value, _studentMainViewModel));
            ReserveBookCommand = new RelayCommand<int?>((bookId) => bookId != null && bookId > 0, (bookId) => ReserveBook(bookId.Value));
            NextPageCommand = new RelayCommand<object>((p) => CanGoToNextPage(), (p) => UpdatePage(CurrentPage + 1));
            PreviousPageCommand = new RelayCommand<object>((p) => CanGoToPreviousPage(), (p) => UpdatePage(CurrentPage - 1));

            LoadCategories();
            LoadBooks();
        }
        private bool CanGoToNextPage() => CurrentPage < TotalPages;
        private bool CanGoToPreviousPage() => CurrentPage > 1;
        private void UpdatePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages)
            {
                CurrentPage = newPage;
            }
        }

        private IEnumerable<Book> FilterAndSortBooks(IEnumerable<Book> allBooks)
        {
            if (!string.IsNullOrWhiteSpace(SearchTextTitle))
            {
                allBooks = allBooks.Where(b => b.Title.Contains(SearchTextTitle, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(SearchTextAuthor))
            {
                allBooks = allBooks.Where(b => b.Authors.Any(a => a.AuthorName.Contains(SearchTextAuthor, StringComparison.OrdinalIgnoreCase)));
            }
            if (SelectedSortCategory != "Default")
            {
                allBooks = allBooks.Where(b => b.Category != null && b.Category.CategoryName == SelectedSortCategory);
            }

            return SelectedSortOrder switch
            {
                "A-Z" => allBooks.OrderBy(b => b.Title),
                "Z-A" => allBooks.OrderByDescending(b => b.Title),
                "New Release" => allBooks.OrderByDescending(b => b.BookId),
                "Most Borrowed" => allBooks.OrderByDescending(b => b.BorrowBooks.Count),
                _ => allBooks
            };
        }

        public void LoadBooks()
        {
            var allBooks = GetBooksFromDatabase();
            allBooks = FilterAndSortBooks(allBooks);
            TotalPages = (int)Math.Ceiling(allBooks.Count() / 12.0) == 0 ? 1 : (int)Math.Ceiling(allBooks.Count() / 12.0);
            BooksList = new ObservableCollection<Book>(allBooks.Skip((CurrentPage - 1) * 12).Take(12));
        }

        private void ViewDetail(int bookId, MainViewModel mainViewModel)
        {
            var book = GetBookById(bookId);
            if (book != null)
            {
                new BookDetailViewModel(mainViewModel, book);
            }
        }

        private ObservableCollection<string> LoadCategoriesFromDatabase()
        {
            using (var context = new LibraryManageSystemContext())
            {
                var categories = context.Categories.Select(c => c.CategoryName).ToList();
                categories.Insert(0, "Default");
                return new ObservableCollection<string>(categories);
            }
        }

        private void LoadCategories() => CategoriesList = LoadCategoriesFromDatabase();

        private Book GetBookById(int bookId)
        {
            using (var context = new LibraryManageSystemContext())
            {
                return context.Books.Include(b => b.Authors).Include(b => b.Category).FirstOrDefault(b => b.BookId == bookId);
            }
        }

        private void ReserveBook(int bookId)
        {
			var result = MessageBox.Show(
					$"Are you sure you want to reserve this book ?",
					"Reservation Book",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question
				);
            if (result == MessageBoxResult.Yes)
            {
                using (var context = new LibraryManageSystemContext())
                {
                    var currentUser = SessionManager.CurrentUser;
                    int borrowCount = context.BorrowBooks.Where(b => b.Status == 1 || b.Status == 0).Where(b => b.UserId == currentUser.UserId).ToList().Count();
                    if (borrowCount < 3)
                    {
                        var borrowBook = new BorrowBook { BookId = bookId, ReservationDate = DateTime.Now, Status = 0, UserId = currentUser.UserId };
                        context.BorrowBooks.Add(borrowBook);
                        context.SaveChanges();
                        MessageBox.Show("Đặt mượn thành công");
                    }
                    else
                    {
                        MessageBox.Show("Lượt đặt mượn đã tới giới hạn, bạn phải trả 1 cuốn sách thì mới có thể đặt mượn tiếp");
                    }
                }
            }
        }

        private IEnumerable<Book> GetBooksFromDatabase()
        {
            using (var context = new LibraryManageSystemContext())
            {
                return context.Books.Include(b => b.Authors).Include(b => b.Category).ToList();
            }
        }
    }
    public class StudentFinesViewModel : BaseViewModel
    {
        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                LoadFines();
            }
        }

        private int _totalPages;
        public int TotalPages { get => _totalPages; set { _totalPages = value; OnPropertyChanged(); } }

        private string _searchTextTitle;
        public string SearchTextTitle
        {
            get => _searchTextTitle;
            set
            {
                _searchTextTitle = value;
                OnPropertyChanged();
                LoadFines();
            }
        }
        private string _selectedAmountRange = "Default";
        public string SelectedAmountRange
        {
            get => _selectedAmountRange;
            set
            {
                _selectedAmountRange = value;
                OnPropertyChanged();
                LoadFines();
            }
        }
        private ObservableCollection<Fine> _finesList;
        public ObservableCollection<Fine> FinesList
        {
            get => _finesList;
            set
            {
                _finesList = value;
                OnPropertyChanged();
            }
        }
        public ICommand ReportFineCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }

        public StudentFinesViewModel()
        {
			ReportFineCommand = new RelayCommand<int?>((fineId) =>
            {
                return fineId.HasValue && fineId > 0;
            }, (fineId) =>
            {
                if (fineId.HasValue)
                {
                    ReportFine reportWindow = new ReportFine();
                    reportWindow.DataContext = new ReportFineViewModel(fineId.Value);
                    reportWindow.ShowDialog();
                }
            });

            NextPageCommand = new RelayCommand<object>((p) => CanGoToNextPage(), (p) => UpdatePage(CurrentPage + 1));
            PreviousPageCommand = new RelayCommand<object>((p) => CanGoToPreviousPage(), (p) => UpdatePage(CurrentPage - 1));

            LoadFines();
        }
        private bool CanGoToNextPage() => CurrentPage < TotalPages;
        private bool CanGoToPreviousPage() => CurrentPage > 1;
        private void UpdatePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages)
            {
                CurrentPage = newPage;
            }
        }


        public void LoadFines()
        {
            using (var context = new LibraryManageSystemContext())
            {
                var currentUser = SessionManager.CurrentUser;

                var query = context.Fines.Include(b => b.Borrow).ThenInclude(b => b.Book)
                .Where(b => b.Borrow.UserId == currentUser.UserId).Where(b => b.Status == 0);
                if (!string.IsNullOrWhiteSpace(SearchTextTitle))
                {
                    var searchTitleLower = SearchTextTitle.ToLower();
                    query = query.Where(b => b.Borrow.Book.Title.ToLower().Contains(searchTitleLower));
                }
                switch (SelectedAmountRange)
                {
                    case "0 - 10.000VND":
                        query = query.Where(f => f.FineAmount >= 0 && f.FineAmount <= 10000);
                        break;
                    case "10.000 - 50.000VND":
                        query = query.Where(f => f.FineAmount > 10000 && f.FineAmount <= 50000);
                        break;
                    case ">50.000VND":
                        query = query.Where(f => f.FineAmount > 50000);
                        break;
                }

                var allFines = query.ToList();
                TotalPages = (int)Math.Ceiling(allFines.Count() / 20.0) == 0 ? 1 : (int)Math.Ceiling(allFines.Count() / 20.0);

                FinesList = new ObservableCollection<Fine>(query.Skip((CurrentPage - 1) * 20).Take(20));

            }
        }
    }
    public class MyBooksViewModel : BaseViewModel
    {
        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                LoadBorrowBooks();
            }
        }
        private int _totalPages;
        public int TotalPages { get => _totalPages; set { _totalPages = value; OnPropertyChanged(); } }

        private string _searchTextTitle;
        public string SearchTextTitle
        {
            get => _searchTextTitle;
            set
            {
                _searchTextTitle = value;
                OnPropertyChanged();
                LoadBorrowBooks();
            }
        }
        private ObservableCollection<BorrowBook> _borrowedBooksList;
        public ObservableCollection<BorrowBook> BorrowedBooksList
        {
            get => _borrowedBooksList;
            set
            {
                _borrowedBooksList = value;
                OnPropertyChanged();
            }
        }
        private string _selectedSortStatus = "Default";
        public string SelectedSortStatus
        {
            get => _selectedSortStatus;
            set { _selectedSortStatus = value; OnPropertyChanged(); LoadBorrowBooks(); }
        }
        private string _selectedSortBorrowDate = "Default";
        public string SelectedSortBorrowDate
        {
            get => _selectedSortBorrowDate;
            set { _selectedSortBorrowDate = value; OnPropertyChanged(); LoadBorrowBooks(); }
        }
        public ICommand ViewDetailCommand { get; set; }
        public ICommand ReportBrrowBookCommand { get; set; }
        public ICommand NextPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }

        public MyBooksViewModel(MainViewModel _studentMainViewModel)
        {

            ViewDetailCommand = new RelayCommand<int?>((bookId) =>
            {
                return bookId != null && bookId > 0;
            }, (bookId) =>
            {
                var book = GetBookById(bookId.Value);
                if (book != null)
                {
                    new BookDetailViewModel(_studentMainViewModel, book);
                }
            });
            ReportBrrowBookCommand = new RelayCommand<int?>((borrowId) =>
            {
                return borrowId.HasValue && borrowId > 0;
            }, (borrowId) =>
            {
                if (borrowId.HasValue)
                {
                    ReportBorrowedBook reportWindow = new ReportBorrowedBook();
                    reportWindow.DataContext = new ReportBorrowedBookViewModel(borrowId.Value);
                    reportWindow.ShowDialog();
                }
            });

            NextPageCommand = new RelayCommand<object>((p) => CanGoToNextPage(), (p) => UpdatePage(CurrentPage + 1));
            PreviousPageCommand = new RelayCommand<object>((p) => CanGoToPreviousPage(), (p) => UpdatePage(CurrentPage - 1));
            LoadBorrowBooks();
        }

        private bool CanGoToNextPage() => CurrentPage < TotalPages;
        private bool CanGoToPreviousPage() => CurrentPage > 1;
        private void UpdatePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages)
            {
                CurrentPage = newPage;
            }
        }

        public void LoadBorrowBooks()
        {
            var allBorrowBooks = GetBorrowedBooksFromDatabase().ToList();
            TotalPages = (int)Math.Ceiling(allBorrowBooks.Count() / 20.0) == 0 ? 1 : (int)Math.Ceiling(allBorrowBooks.Count() / 20.0);
            BorrowedBooksList = new ObservableCollection<BorrowBook>(allBorrowBooks.Skip((CurrentPage - 1) * 20).Take(20));
        }

        private IEnumerable<BorrowBook> GetBorrowedBooksFromDatabase()
        {
            using (var context = new LibraryManageSystemContext())
            {
                var currentUser = SessionManager.CurrentUser;

                var query = context.BorrowBooks.Include(b => b.Book)
                .Where(b => b.UserId == currentUser.UserId);
                if (!string.IsNullOrWhiteSpace(SearchTextTitle))
                {
                    var searchTitleLower = SearchTextTitle.ToLower();
                    query = query.Where(b => b.Book.Title.Contains(searchTitleLower));
                }
                if (SelectedSortStatus != "Default")
                {
                    query = SelectedSortStatus switch
                    {
                        "Reservation" => query.Where(b => b.Status == 0),
                        "Still Borrowing" => query.Where(b => b.Status == 1),
                        "Return On Time" => query.Where(b => b.Status == 5),
                        "Lost" => query.Where(b => b.Status == 4),
                        "Return Late" => query.Where(b => b.Status == 3),
                        "Cancel" => query.Where(b => b.Status == 2),
                        _ => query
                    };
                }
                if (SelectedSortBorrowDate != "Default")
                {
                    DateTime startDate = DateTime.MinValue;

                    switch (SelectedSortBorrowDate)
                    {
                        case "Last 1 Week":
                            startDate = DateTime.Now.AddDays(-7);
                            break;
                        case "Last 1 Month":
                            startDate = DateTime.Now.AddMonths(-1);
                            break;
                        case "Last 1 Year":
                            startDate = DateTime.Now.AddYears(-1);
                            break;
                    }

                    query = query.Where(b => b.BorrowDate >= startDate);
                }
                return query.ToList();
                
            }
        }
        private Book GetBookById(int bookId)
        {
            using (var context = new LibraryManageSystemContext())
            {
                return context.Books.Include(b => b.Authors)
                            .Include(b => b.Category)
                            .FirstOrDefault(b => b.BookId == bookId);
            }

        }
    }
    public class StudentProfileViewModel : BaseViewModel
    {
        private string _Image;
        public string Image { get => _Image; set { _Image = value; OnPropertyChanged(); } }
        private string _StudentId;
        public string StudentId { get => _StudentId; set { _StudentId = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }
        private string _StudentCode;
        public string StudentCode { get => _StudentCode; set { _StudentCode = value; OnPropertyChanged(); } }
        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }
        private string _Fullname;
        public string Fullname { get => _Fullname; set { _Fullname = value; OnPropertyChanged(); } }

        public ICommand UpdateProfileCommand { get; set; }

        public StudentProfileViewModel(MainViewModel mainViewModel)
        {
            LoadUserInfor();

            UpdateProfileCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                UpdateProfile UpdateProfileWindow = new UpdateProfile();
                UpdateProfileWindow.DataContext = new UpdateProfileViewModel(mainViewModel);
                UpdateProfileWindow.ShowDialog();
            });

        }
        public void LoadUserInfor()
        {
            var currentUser = SessionManager.CurrentUser;

            if (currentUser != null)
            {
                Image = currentUser.Image;
                Email = currentUser.Email;
                StudentCode = currentUser.StudentCode;
                Phone = currentUser.Phone;
                Fullname = currentUser.Fullname;
                StudentId = currentUser.UserId.ToString();
            }
        }

    }
    public class UpdateProfileViewModel :BaseViewModel
    {
        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }
        private string _StudentCode;
        public string StudentCode { get => _StudentCode; set { _StudentCode = value; OnPropertyChanged(); } }
        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }
        private string _Fullname;
        public string Fullname { get => _Fullname; set { _Fullname = value; OnPropertyChanged(); } }

        private string _PhoneError;
        public string PhoneError { get => _PhoneError; set { _PhoneError = value; OnPropertyChanged(); } }
        private string _FullnameError;
        public string FullnameError { get => _FullnameError; set { _FullnameError = value; OnPropertyChanged(); } }
        private string _ImageError;
        public string ImageError { get => _ImageError; set { _ImageError = value; OnPropertyChanged(); } }

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

        public ICommand SelectImageCommand { get; set; }
        public ICommand SelectImageCommand1 { get; set; }
        public ICommand UpdateProfileCommand { get; set; }
        public UpdateProfileViewModel(MainViewModel mainViewModel)
        {
            var currentUser = SessionManager.CurrentUser;
            if(currentUser != null)
            {
                Email = currentUser.Email;
                Phone = currentUser.Phone;
                StudentCode = currentUser.StudentCode;
                Fullname = currentUser.Fullname;
            }
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

            UpdateProfileCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                UpdateStudentProfile(mainViewModel);
            });
        }
        public async void UpdateStudentProfile(MainViewModel mainViewModel)
        {

			if (string.IsNullOrWhiteSpace(Phone))
            {
                PhoneError = "Phone không được để trống.";
                ImageError = "";
                return;
            }
            if (Phone.Length != 10)
            {
                PhoneError = "Phone phải gồm 10 kí tự số.";
                ImageError = "";
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
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message);
                }
            }

            try
            {
                using (var db = new LibraryManageSystemContext())
                {
                    var currentUser = SessionManager.CurrentUser;
                    var userToUpdate = db.Users.FirstOrDefault(u => u.UserId == currentUser.UserId);

                    if (userToUpdate != null) 
                    {
                        userToUpdate.Phone = Phone;
                        if(ImageUrl != null)
                        {
							userToUpdate.Image = ImageUrl;
						}
						userToUpdate.Fullname = Fullname;
                        db.SaveChanges();
					}
					currentUser.Image = ImageUrl;
                    currentUser.Phone = Phone;
                    currentUser.Fullname = Fullname;
                    MessageBox.Show("Update profile success.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Application.Current.Windows.OfType<UpdateProfile>().FirstOrDefault()?.Close();
					mainViewModel.CurrentView = new StudentProfileViewModel(mainViewModel);
				}
			}
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi update: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    public class BookDetailViewModel : BaseViewModel
    {
        private Book _selectedBook;

        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
            }
        }
        public ICommand ReserveBookCommand { get; set; }

        public BookDetailViewModel(MainViewModel _studentMainViewModel, Book book)
        {
            SelectedBook = book;
            var bookDetailView = new BookDetail();
            bookDetailView.DataContext = this;
            _studentMainViewModel.CurrentView = bookDetailView;


            ReserveBookCommand = new RelayCommand<int?>((bookId) =>
                {
                    return bookId != null && bookId > 0;
                }, (bookId) =>
                {
                    ReserveBook(bookId.Value);
                });
        }
        private void ReserveBook(int bookId)
        {
			var result = MessageBox.Show(
					$"Are you sure you want to reserve this book ?",
					"Reservation Book",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question
				);
            if (result == MessageBoxResult.Yes)
            {
                using (var context = new LibraryManageSystemContext())
                {
                    var currentUser = SessionManager.CurrentUser;
                    int borrowCount = context.BorrowBooks.Where(b => b.Status == 1 || b.Status == 0).Where(b => b.UserId == currentUser.UserId).ToList().Count();
                    if (borrowCount < 3)
                    {
                        var borrowBook = new BorrowBook { BookId = bookId, ReservationDate = DateTime.Now, Status = 0, UserId = currentUser.UserId };
                        context.BorrowBooks.Add(borrowBook);
                        context.SaveChanges();
                        MessageBox.Show("Đặt mượn thành công");
                    }
                    else
                    {
                        MessageBox.Show("Lượt đặt mượn đã tới giới hạn, bạn phải trả 1 cuốn sách thì mới có thể đặt mượn tiếp");
                    }
                }
            }
        }
    }
    public class StudentFinesHistoryViewModel : BaseViewModel
    {
        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                LoadFinesHistory();
            }
        }

        private int _totalPages;
        public int TotalPages { get => _totalPages; set { _totalPages = value; OnPropertyChanged(); } }
        private string _searchTextTitle;
        public string SearchTextTitle
        {
            get => _searchTextTitle;
            set
            {
                _searchTextTitle = value;
                OnPropertyChanged();
                LoadFinesHistory();
            }
        }
        private string _selectedAmountRange = "Default";
        public string SelectedAmountRange
        {
            get => _selectedAmountRange;
            set
            {
                _selectedAmountRange = value;
                OnPropertyChanged();
                LoadFinesHistory();
            }
        }
        private string _selectedPayDate = "Default";
        public string SelectedPayDate
        {
            get => _selectedPayDate;
            set
            {
                _selectedPayDate = value;
                OnPropertyChanged();
                LoadFinesHistory();
            }
        }
        private ObservableCollection<Fine> _finesHistoryList;
        public ObservableCollection<Fine> FinesHistoryList
        {
            get => _finesHistoryList;
            set
            {
                _finesHistoryList = value;
                OnPropertyChanged();
            }
        }

        public ICommand NextPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }

        public StudentFinesHistoryViewModel()
        {
            NextPageCommand = new RelayCommand<object>((p) => CanGoToNextPage(), (p) => UpdatePage(CurrentPage + 1));
            PreviousPageCommand = new RelayCommand<object>((p) => CanGoToPreviousPage(), (p) => UpdatePage(CurrentPage - 1));
            LoadFinesHistory();
        }
        private bool CanGoToNextPage() => CurrentPage < TotalPages;
        private bool CanGoToPreviousPage() => CurrentPage > 1;
        private void UpdatePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages)
            {
                CurrentPage = newPage;
            }
        }
        public void LoadFinesHistory()
        {
            using (var context = new LibraryManageSystemContext())
            {
                var currentUserSession = SessionManager.CurrentUser;
                var currentUserId = 2;
                var query = context.Fines.Include(b => b.Borrow).ThenInclude(b => b.Book)
                .Where(b => b.Borrow.UserId == currentUserSession.UserId).Where(b => b.Status == 1);
                if (!string.IsNullOrWhiteSpace(SearchTextTitle))
                {
                    query = query.Where(b => b.Borrow.Book.Title.Contains(SearchTextTitle, StringComparison.OrdinalIgnoreCase));
                }
                switch (SelectedAmountRange)
                {
                    case "0 - 10.000VND":
                        query = query.Where(f => f.FineAmount >= 0 && f.FineAmount <= 10000);
                        break;
                    case "10.000 - 50.000VND":
                        query = query.Where(f => f.FineAmount > 10000 && f.FineAmount <= 50000);
                        break;
                    case ">50.000VND":
                        query = query.Where(f => f.FineAmount > 50000);
                        break;
                }
                if (SelectedPayDate != "Default")
                {
                    DateTime startDate = DateTime.MinValue;

                    switch (SelectedPayDate)
                    {
                        case "Last 1 Week":
                            startDate = DateTime.Now.AddDays(-7);
                            break;
                        case "Last 1 Month":
                            startDate = DateTime.Now.AddMonths(-1);
                            break;
                        case "Last 1 Year":
                            startDate = DateTime.Now.AddYears(-1);
                            break;
                    }

                    query = query.Where(b => b.PaidDate >= startDate);
                }

                var allFinesHistory = query.ToList();
                TotalPages = (int)Math.Ceiling(allFinesHistory.Count() / 20.0) == 0 ? 1 : (int)Math.Ceiling(allFinesHistory.Count() / 20.0);
                FinesHistoryList = new ObservableCollection<Fine>(allFinesHistory.Skip((CurrentPage - 1) * 20).Take(20));
            }
        }
    }
    public class ReportsViewModel : BaseViewModel
    {
        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                LoadReports();
            }
        }
        private int _totalPages;
        public int TotalPages { get => _totalPages; set { _totalPages = value; OnPropertyChanged(); } }


        private ObservableCollection<Report> _reportsList;
        public ObservableCollection<Report> ReportsList
        {
            get => _reportsList;
            set
            {
                _reportsList = value;
                OnPropertyChanged();
            }
        }
        public ICommand NextPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }

        public ReportsViewModel()
        {
            NextPageCommand = new RelayCommand<object>((p) => CanGoToNextPage(), (p) => UpdatePage(CurrentPage + 1));
            PreviousPageCommand = new RelayCommand<object>((p) => CanGoToPreviousPage(), (p) => UpdatePage(CurrentPage - 1));

            LoadReports();
        }

        private bool CanGoToNextPage() => CurrentPage < TotalPages;
        private bool CanGoToPreviousPage() => CurrentPage > 1;
        private void UpdatePage(int newPage)
        {
            if (newPage >= 1 && newPage <= TotalPages)
            {
                CurrentPage = newPage;
            }
        }

        public void LoadReports()
        {
            var allReports = GetReportsFromDatabase().ToList();
            TotalPages = (int)Math.Ceiling(allReports.Count() / 20.0) == 0 ? 1 : (int)Math.Ceiling(allReports.Count() / 20.0);
            ReportsList = new ObservableCollection<Report>(allReports.Skip((CurrentPage - 1) * 12).Take(12));
        }
        private IEnumerable<Report> GetReportsFromDatabase()
        {
            using (var context = new LibraryManageSystemContext())
            {
                return context.Reports.Include(b => b.Borrow).ThenInclude(b => b.Book).ToList();
            }
        }
    }
    public class ReportBorrowedBookViewModel : BaseViewModel
    {
        private string _BookBorrowedTitle;
        public string BookBorrowedTitle { get => _BookBorrowedTitle; set { _BookBorrowedTitle = value; OnPropertyChanged(); } }
        private int? _bookBorrowedStatus;
        public int? BookBorrowedStatus
        {
            get => _bookBorrowedStatus;
            set
            {
                _bookBorrowedStatus = value;
                OnPropertyChanged();
                UpdateReportReasons();
            }
        }
        private string _bookBorrowedStatusString;
        public string BookBorrowedStatusString { get => _bookBorrowedStatusString; set { _bookBorrowedStatusString = value; OnPropertyChanged(); } }
        private string _LibrarianInCharge;
        public string LibrarianInCharge { get => _LibrarianInCharge; set { _LibrarianInCharge = value; OnPropertyChanged(); } }
        private string _selectedReportReason;
        public string SelectedReportReason
        {
            get => _selectedReportReason;
            set { _selectedReportReason = value; OnPropertyChanged(); }
        }
        public ICommand SaveReportCommand { get; set; }
        public ReportBorrowedBookViewModel(int borrowId)
        {
            SaveReportCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SaveReport(borrowId);
            });

            var borrowBook = GetBorrowBookById(borrowId);
            if (borrowBook != null)
            {
                BookBorrowedTitle = borrowBook.Book.Title;
                BookBorrowedStatus = borrowBook.Status;
                BookBorrowedStatusString = borrowBook.StatusString;
                LibrarianInCharge = borrowBook.LibrarianInCharge;
            }

        }
        private void SaveReport(int borrowIdReport)
        {
            using (var context = new LibraryManageSystemContext())
            {
                var currentUser = SessionManager.CurrentUser;
                var report = new Report
                {
                    ReportReason = SelectedReportReason,   
                    UserId = currentUser.UserId,
                    Status = "processing",
                    BorrowId = borrowIdReport,
                    CreatedAt = DateTime.Now
                };

                context.Reports.Add(report);
                context.SaveChanges();
            }
            MessageBox.Show("Report your borrow successfully!");
            Application.Current.Windows.OfType<ReportBorrowedBook>().FirstOrDefault()?.Close();
        }
        private BorrowBook GetBorrowBookById(int borrowId)
        {
            using (var context = new LibraryManageSystemContext())
            {
                return context.BorrowBooks.Include(b => b.Book).FirstOrDefault(b => b.BorrowId == borrowId);
            }
        }
        public ObservableCollection<string> ReportReasons { get; } = new ObservableCollection<string>();

       
        private void UpdateReportReasons()
        {
            ReportReasons.Clear();
            if (BookBorrowedStatus == 1) //Borrowing
            {
                ReportReasons.Add("Incorrect book borrowed");
                ReportReasons.Add("I lost lhe book");
                ReportReasons.Add("Wrong borrow status");
            }
            else if (BookBorrowedStatus == 5) //Return on time
            {
                ReportReasons.Add("Wrong borrow status");
                ReportReasons.Add("I Want to extend Borrow Time");
            }
            else if (BookBorrowedStatus == 4) //Lost/Not return
            {
                ReportReasons.Add("Wrong borrow status");

            }
            else if (BookBorrowedStatus == 3) //Overdue
            {
                ReportReasons.Add("Wrong borrow status");

            }
        }
    }
    public class ReportFineViewModel : BaseViewModel
    {
        private string _fineType;
        public string FineType { get => _fineType; set { _fineType = value; OnPropertyChanged(); } }

        private string _fineStatus;
        public string FineStatus
        {
            get => _fineStatus;
            set
            {
                _fineStatus = value;
                OnPropertyChanged();
                ReprotFineReason();
            }
        }
        private string _selectedReportFineReason;
        public string SelectedReportFineReason
        {
            get => _selectedReportFineReason;
            set { _selectedReportFineReason = value; OnPropertyChanged(); }
        }

        public ICommand SaveReportFineCommand { get; set; }
        public ReportFineViewModel(int fineId)
        {
            SaveReportFineCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                SaveReport(fineId);
            });

            var fine = GetFineById(fineId);
            if (fine != null)
            {
                FineType = fine.FineType;
                FineStatus = fine.Status.ToString();
            }

        }
        private void SaveReport(int fineIdReport)
        {
            using (var context = new LibraryManageSystemContext())
            {
                var currentUser = SessionManager.CurrentUser;
                var report = new Report
                {
                    ReportReason = SelectedReportFineReason,
                    UserId = currentUser.UserId,
                    Status = "processing",
                    FineId = fineIdReport,
                    CreatedAt = DateTime.Now
                };

                context.Reports.Add(report);
                context.SaveChanges();
            }
            MessageBox.Show("Report your fine successfully!");
            Application.Current.Windows.OfType<ReportFine>().FirstOrDefault()?.Close();
        }
        private Fine GetFineById(int fineId)
        {
            using (var context = new LibraryManageSystemContext())
            {
                return context.Fines.FirstOrDefault(b => b.FineId == fineId);
            }
        }
        public ObservableCollection<string> ReportFineReasons { get; } = new ObservableCollection<string>();
        private void ReprotFineReason()
        {
            ReportFineReasons.Add("Incorrect fines");
            ReportFineReasons.Add("Lost book charge disputed");
            ReportFineReasons.Add("Book marked as damaged before borrowing");
            ReportFineReasons.Add("Fine amount does not match library policy");
            ReportFineReasons.Add("Multiple fines for the same overdue book");

        }
    }
}