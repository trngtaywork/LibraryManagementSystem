using LibraryManagementWpf.DTO;
using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Admin;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Theraot;

namespace LibraryManagementWpf.ViewModel.Admin
{
	public class DashboardViewModel : BaseViewModel
	{
		private string _TotalCategory;
		public string TotalCategory { get => _TotalCategory; set { _TotalCategory = value; OnPropertyChanged(); } }
		private string _TotalBook;
		public string TotalBook { get => _TotalBook; set { _TotalBook = value; OnPropertyChanged(); } }
		private string _TotalBookRecords;
		public string TotalBookRecords { get => _TotalBookRecords; set { _TotalBookRecords = value; OnPropertyChanged(); } }
		private string _TotalAccount;
		public string TotalAccount { get => _TotalAccount; set { _TotalAccount = value; OnPropertyChanged(); } }
		private string _TotalFine;
		public string TotalFine { get => _TotalFine; set { _TotalFine = value; OnPropertyChanged(); } }
		private string _DisplayTotalPage;
		public string DisplayTotalPage { get => _DisplayTotalPage; set { _DisplayTotalPage = value; OnPropertyChanged(); } }
		public ICommand DashboardAllCommand { get; set; }
		public ICommand DashboardTodayCommand { get; set; }
		public ICommand DashboardLastWeekCommand { get; set; }
		public ICommand DashboardLastMonthCommand { get; set; }
		public ICommand DashboardLastYearCommand { get; set; }
		private ObservableCollection<BookDTO> _Books;
		public ObservableCollection<BookDTO> Books { get => _Books; set { _Books = value; OnPropertyChanged(); } }
		private ObservableCollection<Fine> _ListFine;
		public ObservableCollection<Fine> ListFine { get => _ListFine; set { _ListFine = value; OnPropertyChanged(); } }
		private ObservableCollection<decimal> _FineAmounts;

		public ObservableCollection<decimal> FineAmounts
		{
			get => _FineAmounts;
			set
			{
				_FineAmounts = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<BorrowBookDetails> _ListBorrow;
		public ObservableCollection<BorrowBookDetails> ListBorrow { get => _ListBorrow; set { _ListBorrow = value; OnPropertyChanged(); } }
		public ObservableCollection<BookDTO> DisplayedListPerPage { get; private set; }

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
					LoadDataBookPagination();
				}
			}
		}

		private int _TotalPages = 1;
		public int TotalPages
		{
			get => _TotalPages;
			set
			{
				if (_TotalPages != value)
				{
					_TotalPages = value;
					OnPropertyChanged(nameof(TotalPages));
					LoadDataBookPagination();
				}
			}
		}
	
		public ICommand NextPageCommand { get; }
		public ICommand PreviousPageCommand { get; }

		public void LoadDataBookPagination()
		{
			var skipItems = (CurrentPage - 1) * _itemPerPage;
			DisplayedListPerPage = new ObservableCollection<BookDTO>(Books.Skip(skipItems).Take(_itemPerPage));
			OnPropertyChanged(nameof(DisplayedListPerPage));
			TotalPages = (int)Math.Ceiling((double)Books.Count / _itemPerPage);
			OnPropertyChanged(nameof(TotalPages));
			DisplayTotalPage = $"Showing {skipItems} to {skipItems + _itemPerPage} of {TotalPages} entries";
		}

		public bool IsPreviousPageEnabled;
		public bool IsNextPageEnabled;

		public DashboardViewModel() 
		{
			LoadDataAllDashboard();
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
			LoadDataBookPagination();

			DashboardAllCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				LoadDataAllDashboard();
				CurrentPage = 1;
				LoadDataBookPagination();
			});
			DashboardTodayCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				LoadDataTodayDashboard();
				CurrentPage = 1;
				LoadDataBookPagination();
			});
			DashboardLastMonthCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				LoadDataLastMonthDashboard();
				CurrentPage = 1;
				LoadDataBookPagination();
			});
			DashboardLastWeekCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				LoadDataLastWeekDashboard();
				CurrentPage = 1;
				LoadDataBookPagination();
			});
			DashboardLastYearCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				LoadDataLastYearDashboard();
				CurrentPage = 1;
				LoadDataBookPagination();
			});
		}
		
		public void LoadFineAmounts()
		{
			if (ListFine != null)
			{
				FineAmounts = new ObservableCollection<decimal>(
					ListFine.Select(f => f.FineAmount)
							.Where(amount => amount.HasValue)
							.Select(amount => amount.Value)
				);
			}
		}
		
		public void LoadDataAllDashboard()
		{
			using (var _context = new LibraryManageSystemContext())
			{
				Books = new ObservableCollection<BookDTO>();
				var query = _context.Books.Include(b => b.Authors).Include(b => b.Category).AsQueryable();
				var booksFromDb = query.ToList();
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

				ListBorrow = new ObservableCollection<BorrowBookDetails>(
					_context.BorrowBooks
						.OrderByDescending(b => b.BorrowDate)
						.Take(4)
						.Join(_context.Users,
							borrow => borrow.UserId,
							user => user.UserId,
							(borrow, user) => new { borrow, user })
						.Join(_context.Books,
							borrowUser => borrowUser.borrow.BookId,
							book => book.BookId,
							(borrowUser, book) => new BorrowBookDetails
							{
								BorrowId = borrowUser.borrow.BorrowId,
								BorrowDate = borrowUser.borrow.BorrowDate,
								DueDate = borrowUser.borrow.DueDate,
								ReturnDate = borrowUser.borrow.ReturnDate,
								Status = borrowUser.borrow.Status,
								UserFullName = borrowUser.user.Fullname,
								BookTitle = book.Title
							})
						.ToList()
				);

				TotalBook = _context.Books.Sum(b => b.Quantity).ToString("N0");
				TotalBookRecords = _context.Books.Count().ToString();

				TotalAccount = _context.Users
					.Count().ToString();

				TotalCategory = _context.Categories
					.Count().ToString();

				TotalFine = _context.Fines.Where(f => f.Status == 1)
				.Count().ToString();

				ListFine = new ObservableCollection<Fine>(_context.Fines.Where(f => f.Status == 1));
			}
			LoadFineAmounts();
		}
		public void LoadDataTodayDashboard()
		{

			var now = DateTime.Now;
			using (var _context = new LibraryManageSystemContext())
			{
				Books = new ObservableCollection<BookDTO>();
				var query = _context.Books.Include(b => b.Authors).Include(b => b.Category).Where(b => b.CreatedAt == now.Date).AsQueryable();
				var booksFromDb = query.ToList();
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

				TotalBook = _context.Books.Where(b => b.CreatedAt == now.Date)
					.Sum(b => b.Quantity).ToString("N0").ToString();
				TotalBookRecords = _context.Books.Where(b => b.CreatedAt == now.Date).Count().ToString();

				TotalAccount = _context.Users.Where(u => u.CreateAt == now.Date)
					.Count().ToString();

				TotalCategory = _context.Categories.Where(c=>c.CreatedAt == now.Date)
					.Count().ToString();
				TotalFine = _context.Fines.Where(f => f.PaidDate == now.Date && f.Status == 1)
				.Count().ToString();
				ListFine = new ObservableCollection<Fine>(_context.Fines.Where(f => f.PaidDate == now.Date && f.Status == 1));
			}
			LoadFineAmounts();
		}

		public void LoadDataLastYearDashboard()
		{
			var now = DateTime.Now;
			using (var _context = new LibraryManageSystemContext())
			{
				Books = new ObservableCollection<BookDTO>();
				var query = _context.Books.Include(b => b.Authors).Include(b => b.Category).Where(b => b.CreatedAt >= now.AddYears(-1)).AsQueryable();
				var booksFromDb = query.ToList();
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

				TotalBook = _context.Books.Where(b => b.CreatedAt >= now.AddYears(-1))
					.Sum(b => b.Quantity).ToString("N0").ToString();
				TotalBookRecords = _context.Books.Where(b => b.CreatedAt >= now.AddYears(-1)).Count().ToString();

				TotalAccount = _context.Users.Where(u => u.CreateAt >= now.AddYears(-1))
					.Count().ToString();

				TotalCategory = _context.Categories.Where(c => c.CreatedAt >= now.AddYears(-1))
					.Count().ToString();
				TotalFine = _context.Fines.Where(f => f.PaidDate >= now.AddYears(-1) && f.Status == 1)
				.Count().ToString();

				ListFine = new ObservableCollection<Fine>(_context.Fines.Where(f => f.PaidDate >= now.AddYears(-1) && f.Status == 1));
			}
			LoadFineAmounts();
		}

		public void LoadDataLastMonthDashboard()
		{
			var now = DateTime.Now;
			using (var _context = new LibraryManageSystemContext())
			{
				Books = new ObservableCollection<BookDTO>();
				var query = _context.Books.Include(b => b.Authors).Include(b => b.Category).Where(b => b.CreatedAt >= now.AddMonths(-1)).AsQueryable();
				var booksFromDb = query.ToList();
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

				TotalBook = _context.Books.Where(b => b.CreatedAt >= now.AddMonths(-1))
					.Sum(b => b.Quantity).ToString("N0").ToString();
				TotalBookRecords = _context.Books.Where(b => b.CreatedAt >= now.AddMonths(-1)).Count().ToString();

				TotalAccount = _context.Users.Where(u => u.CreateAt >= now.AddMonths(-1))
					.Count().ToString();

				TotalCategory = _context.Categories.Where(c => c.CreatedAt >= now.AddMonths(-1))
					.Count().ToString();
				TotalFine = _context.Fines.Where(f => f.PaidDate >= now.AddMonths(-1) && f.Status == 1)
				.Count().ToString();

				ListFine = new ObservableCollection<Fine>(_context.Fines.Where(f => f.PaidDate >= now.AddMonths(-1) && f.Status == 1));
			}
			LoadFineAmounts();
		}

		public void LoadDataLastWeekDashboard()
		{
			var now = DateTime.Now;
			using (var _context = new LibraryManageSystemContext())
			{
				Books = new ObservableCollection<BookDTO>();
				var query = _context.Books.Include(b => b.Authors).Include(b => b.Category).Where(b => b.CreatedAt >= now.AddDays(-7)).AsQueryable();
				var booksFromDb = query.ToList();
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

				TotalBook = _context.Books.Where(b => b.CreatedAt >= now.AddDays(-7))
					.Sum(b => b.Quantity).ToString("N0").ToString();
				TotalBookRecords = _context.Books.Where(b => b.CreatedAt >= now.AddDays(-7)).Count().ToString();

				TotalAccount = _context.Users.Where(u => u.CreateAt >= now.AddDays(-7))
					.Count().ToString();

				TotalCategory = _context.Categories.Where(c => c.CreatedAt >= now.AddDays(-7))
					.Count().ToString();
				TotalFine = _context.Fines.Where(f => f.PaidDate >= now.AddDays(-7) && f.Status == 1)
				.Count().ToString();

				ListFine = new ObservableCollection<Fine>(_context.Fines.Where(f => f.PaidDate >= now.AddDays(-7) && f.Status == 1));
			}
			LoadFineAmounts();
		}
	}
}
