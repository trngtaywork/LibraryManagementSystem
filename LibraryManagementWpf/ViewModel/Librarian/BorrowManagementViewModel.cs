using LibraryManagementWpf.DTO;
using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Librarian;
using LibraryManagementWpf.ViewModel.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementWpf.ViewModel.Librarian
{
	public class BorrowManagementViewModel : BaseViewModel
	{
		public ICommand AddNewBorrowViewCommand { get; set; }
		public ICommand AcceptCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public ICommand ReturnCommand { get; set; }
		public ICommand ReportLostCommand { get; set; }
		public ICommand CancelWindowCommand { get; set; }
		public ICommand AddBorrowCommand { get; set; }
		public ICommand CheckInfoCommand { get; set; }
		public ICommand CategorySelectionChangedCommand { get; set; }


		private ObservableCollection<BorrowBookDTO> _BorrowBooks;
		public ObservableCollection<BorrowBookDTO> BorrowBooks { get => _BorrowBooks; set { _BorrowBooks = value; OnPropertyChanged(); } }

		private int borrowCount;
		public int BorrowCount
		{
			get => borrowCount;
			private set
			{
				borrowCount = value;
				OnPropertyChanged();
			}
		}

		private string _StudentCode;
		public string StudentCode { get => _StudentCode; set { _StudentCode = value; OnPropertyChanged(); } }
		private string _FullName;
		public string FullName { get => _FullName; set { _FullName = value; OnPropertyChanged(); } }
		private string _Phone;
		public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }
		private string _Email;
		public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }
		private string _BookId;
		public string BookId { get => _BookId; set { _BookId = value; OnPropertyChanged(); } }
		private string _CategoryId;
		public string CategoryId { get => _CategoryId; set { _CategoryId = value; OnPropertyChanged(); } }
		private string _StudentCodeError;
		public string StudentCodeError { get => _StudentCodeError; set { _StudentCodeError = value; OnPropertyChanged(); } }
		private string _CategoryError;
		public string CategoryError { get => _CategoryError; set { _CategoryError = value; OnPropertyChanged(); } }
		private string _BookError;
		public string BookError { get => _BookError; set { _BookError = value; OnPropertyChanged(); } }

		private ObservableCollection<Category> _ListCategory;
		public ObservableCollection<Category> ListCategory { get => _ListCategory; set { _ListCategory = value; OnPropertyChanged(); } }
		private ObservableCollection<Book> _ListBook;
		public ObservableCollection<Book> ListBook { get => _ListBook; set { _ListBook = value; OnPropertyChanged(); } }
		public ObservableCollection<string> StatusOptions { get; set; }
		private string selectedBorrowStatus;
		public string SelectedBorrowStatus
		{
			get => selectedBorrowStatus;
			set
			{
				selectedBorrowStatus = value;
				OnPropertyChanged();
				LoadBorrowBooks();
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
				LoadBorrowBooks();
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
				LoadBorrowBooks();
			}
		}
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
		public BorrowManagementViewModel()
		{
			using (var _context = new LibraryManageSystemContext())
			{
				ListCategory = new ObservableCollection<Category>(_context.Categories);
			}
			var currentUserSession = SessionManager.CurrentUser;

			if (currentUserSession != null)
			{
				CurrentUser = currentUserSession;
			}

			BorrowBooks = new ObservableCollection<BorrowBookDTO>();
			StatusOptions = new ObservableCollection<string>
			{
				"All",
				"Reservation",
				"Borrowing",
			};
			SortOptions = new ObservableCollection<string>
			{
				"Oldest",
				"Newest"
			};
			LoadBorrowBooks();

			SelectedSortOption = SortOptions.First();
			SelectedBorrowStatus = StatusOptions.First();

			AddNewBorrowViewCommand = new RelayCommand<object>((p) => true, p =>
			{
				var addBorrowWindow = new AddNewBorrow();
				addBorrowWindow.ShowDialog();
			});
			AcceptCommand = new RelayCommand<BorrowBookDTO>(p => p != null, p => AcceptBorrow(p));
			CancelCommand = new RelayCommand<BorrowBookDTO>(p => p != null, p => CancelBorrow(p));
			ReturnCommand = new RelayCommand<BorrowBookDTO>(p => p != null, p => ReturnBorrow(p));
			ReportLostCommand = new RelayCommand<BorrowBookDTO>(p => p != null, p => ReportLost(p));
			CancelWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
			{
				p.Close();
			});
			CheckInfoCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				CheckInfomationStudent();
			});
			CategorySelectionChangedCommand = new RelayCommand<string>((p) => { return true; }, (p) =>
			{
				if (p != null)
				{
					LoadBooks(int.Parse(p));
				}
			});
			AddBorrowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
			{
				DoAddBorrow(p);
			});
			UpdateOverdueStatus();
		}

		public void DoAddBorrow(Window p)
		{
			if (string.IsNullOrWhiteSpace(StudentCode))
			{
				StudentCodeError = "Hãy nhập vào mã sinh viên.";
				CategoryError = "";
				BookError = "";
				return;
			}

			if (CategoryId == null)
			{
				CategoryError = "Hãy chọn thể loại.";
				StudentCodeError = "";
				BookError = "";
				return;
			}

			if (BookId == null)
			{
				BookError = "Hãy chọn sách.";
				StudentCodeError = "";
				CategoryError = "";
				return;
			}
			StudentCodeError = "";
			CategoryError = "";
			BookError = "";
			var selectedBook = BookId;
			try
			{
				using (var db = new LibraryManageSystemContext())
				{
					var student = db.Users.FirstOrDefault(u => u.StudentCode == StudentCode);
					if (student == null)
					{
						MessageBox.Show("Không tìm thấy sinh viên với mã " + StudentCode, "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}

					var book = db.Books.Find(int.Parse(BookId));
					if (book.Quantity <= 0)
					{
						MessageBox.Show("Cuốn sách này hiện không có sẵn.", "Cảnh báo số lượng", MessageBoxButton.OK, MessageBoxImage.Warning);
						return;
					}

					var borrow = new BorrowBook
					{
						UserId = student.UserId,
						BookId = book.BookId,
						BorrowDate = DateTime.Now,
						DueDate = DateTime.Now.AddDays(15),
						LibrarianInCharge = CurrentUser.Fullname,
						Status = (int)BorrowStatus.Borrowing
					};

					db.BorrowBooks.Add(borrow);
					book.Quantity -= 1;
					db.SaveChanges();
					LoadBorrowBooks();
					MessageBox.Show("Thêm mới yêu cầu mượn sách thành công.");
					p.Close();
				}

				StudentCode = null;
				Phone = null;
				Email = null;
				CategoryId = null;
				BookId = null;
				FullName = null;
			}
			catch (Exception e)
			{
				MessageBox.Show("Lỗi khi thêm yêu cầu mượn sách " + e.Message, "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public void LoadBooks(int categoryId)
		{
			using (var db = new LibraryManageSystemContext())
			{
				var books = db.Books.Where(b => b.CategoryId == categoryId).ToList();
				ListBook = new ObservableCollection<Book>(books);
			}
		}

		public void CheckInfomationStudent()
		{
			if (string.IsNullOrWhiteSpace(StudentCode))
			{
				StudentCodeError = "Hãy nhập vào mã sinh viên.";
				return;
			}
			StudentCodeError = "";
			using (var db = new LibraryManageSystemContext())
			{
				var student = db.Users.FirstOrDefault(u => u.StudentCode == StudentCode);
				if (student != null)
				{
					FullName = student.Fullname;
					Phone = student.Phone;
					Email = student.Email;
				}
				else
				{
					MessageBox.Show("Không tìm thấy sinh viên với mã " + StudentCode, "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		public void LoadBorrowBooks()
		{
			using (var db = new LibraryManageSystemContext())
			{
				var query = db.BorrowBooks
					.Include(b => b.Book)
					.Include(b => b.User)
					.Where(b => b.Status == (int)BorrowStatus.Reservation || b.Status == (int)BorrowStatus.Borrowing)
					.AsQueryable();

				if (!string.IsNullOrEmpty(SearchText))
				{
					query = query.Where(b => b.User.Fullname.Contains(SearchText) ||
											b.User.StudentCode.Contains(SearchText) ||
											b.Book.Title.Contains(SearchText));
				}

				if (SelectedBorrowStatus != "All")
				{
					int statusFilter = SelectedBorrowStatus == "Reservation" ? (int)BorrowStatus.Reservation : (int)BorrowStatus.Borrowing;
					query = query.Where(b => b.Status == statusFilter);
				}

				switch (SelectedSortOption)
				{
					case "Oldest":
						query = query.OrderBy(b => b.BorrowDate);
						break;
					case "Newest":
						query = query.OrderByDescending(b => b.BorrowDate);
						break;
					default:
						break;
				}

				var borrowBooksFromDb = query.ToList();
				BorrowBooks.Clear();
				foreach (var borrowBook in borrowBooksFromDb)
				{
					BorrowBooks.Add(new BorrowBookDTO
					{
						BorrowId = borrowBook.BorrowId,
						StudentCode = borrowBook.User.StudentCode,
						BorrowerName = borrowBook.User.Fullname,
						BookTitle = borrowBook.Book.Title,
						ReservationDate = borrowBook.ReservationDate,
						BorrowDate = borrowBook.BorrowDate,
						DueDate = borrowBook.DueDate,
						LibrarianInCharge = borrowBook.LibrarianInCharge,
						Status = (BorrowStatus)borrowBook.Status
					});
				}
				BorrowCount = BorrowBooks.Count;
			}
		}



		private void AcceptBorrow(BorrowBookDTO borrow)
		{
			using (var db = new LibraryManageSystemContext())
			{
				var borrowToUpdate = db.BorrowBooks.Include(b => b.Book).FirstOrDefault(b => b.BorrowId == borrow.BorrowId);
				if (borrowToUpdate != null && borrowToUpdate.Book.Quantity > 0)
				{
					borrowToUpdate.BorrowDate = DateTime.Now;
					borrowToUpdate.DueDate = borrowToUpdate.BorrowDate?.AddDays(15);
					borrowToUpdate.Status = (int)BorrowStatus.Borrowing;
					borrowToUpdate.Book.Quantity -= 1;
					borrowToUpdate.LibrarianInCharge = CurrentUser.Fullname;
					db.SaveChanges();
					LoadBorrowBooks();
				}
			}
		}

		private void CancelBorrow(BorrowBookDTO borrow)
		{
			using (var db = new LibraryManageSystemContext())
			{
				var borrowToUpdate = db.BorrowBooks.Find(borrow.BorrowId);
				if (borrowToUpdate != null)
				{
					borrowToUpdate.LibrarianInCharge = CurrentUser.Fullname;
					borrowToUpdate.Status = (int)BorrowStatus.Canceled;
					db.SaveChanges();
					LoadBorrowBooks();
				}
			}
		}

		private void ReturnBorrow(BorrowBookDTO borrow)
		{
			using (var db = new LibraryManageSystemContext())
			{
				var borrowToUpdate = db.BorrowBooks.Include(b => b.Book).FirstOrDefault(b => b.BorrowId == borrow.BorrowId);
				if (borrowToUpdate != null)
				{
					borrowToUpdate.LibrarianInCharge = CurrentUser.Fullname;
					borrowToUpdate.Status = (int)BorrowStatus.Returned;
					borrowToUpdate.ReturnDate = DateTime.Now;
					borrowToUpdate.Book.Quantity += 1;
					db.SaveChanges();
					LoadBorrowBooks();
				}
			}
		}

		private void ReportLost(BorrowBookDTO borrow)
		{
			using (var db = new LibraryManageSystemContext())
			{
				EmailSender.EmailSender es = new EmailSender.EmailSender();
                var borrowToUpdate = db.BorrowBooks.Find(borrow.BorrowId);
				if (borrowToUpdate != null)
				{
					borrowToUpdate.Status = (int)BorrowStatus.Lost;
					borrowToUpdate.LibrarianInCharge = CurrentUser.Fullname;
					var fine = new Fine
					{
						BorrowId = borrowToUpdate.BorrowId,
						FineType = "Lost",
						FineAmount = CalculateFineAmount(borrowToUpdate),
						Status = 0,
						PaidDate = null
					};

					db.Fines.Add(fine);
					db.SaveChanges();
					var user = db.Users.Where(u => u.UserId == borrowToUpdate.UserId).FirstOrDefault();
					es.SendEmail(user.Email, "Fine Notify", "fineNotify", borrowToUpdate.BorrowId);
				}
			}
			LoadBorrowBooks();
		}

		private void UpdateOverdueStatus()
		{
			using (var db = new LibraryManageSystemContext())
			{
				var overdueBorrows = db.BorrowBooks
					.Where(b => b.Status == (int)BorrowStatus.Borrowing
								&& b.DueDate < DateTime.Today
								&& b.ReturnDate == null)
					.ToList();

				foreach (var borrow in overdueBorrows)
				{
					borrow.Status = (int)BorrowStatus.Overdue;
					borrow.LibrarianInCharge = CurrentUser.Fullname;
					var fine = new Fine
					{
						BorrowId = borrow.BorrowId,
						FineType = "Overdue",
						FineAmount = CalculateFineAmount(borrow),
						Status = 0,
						PaidDate = null
					};

					db.Fines.Add(fine);
				}

				db.SaveChanges();
			}
			LoadBorrowBooks();
		}

		public decimal CalculateFineAmount(BorrowBook borrowBook)
		{
			decimal lostFine = 100000;

			if (borrowBook.Status == (int)BorrowStatus.Lost)
			{
				return lostFine;
			}
			return 0;
		}
	}
}