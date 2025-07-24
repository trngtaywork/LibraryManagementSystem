using System.Globalization;
using System.Windows.Controls;
using LibraryManagementWpf.DTO;
using LibraryManagementWpf.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using LibraryManagementWpf.Commands;
using System.Windows;
using LibraryManagementWpf.View.Dialogs;


namespace LibraryManagementWpf.View.Librarian
{
    public partial class DashboardLibrarian : UserControl, INotifyPropertyChanged
    {
        private const int ItemsPerPage = 10;
        private int currentPage = 1;
        private int totalItems;

        public ObservableCollection<FineDTO> PaginatedFines { get; set; }

        private ObservableCollection<InfoCardDTO> infoCards;
        private ObservableCollection<BorrowDTO> lastBorrowedBooks;
        public ICommand ViewFineDetailsCommand { get; set; }
        public ICommand DeleteFineCommand { get; set; }
        public ICommand ShowTodayDataCommand { get; set; }
        public ICommand ShowWeekDataCommand { get; set; }
        public ICommand ShowMonthDataCommand { get; set; }
        public ICommand ShowYearDataCommand { get; set; }
		private string _TotalCategory;
		public string TotalCategory { get => _TotalCategory; set { _TotalCategory = value; OnPropertyChanged(); } }
		private string _TotalBook;
		public string TotalBook { get => _TotalBook; set { _TotalBook = value; OnPropertyChanged(); } }
		private string _TotalAccount;
		public string TotalAccount { get => _TotalAccount; set { _TotalAccount = value; OnPropertyChanged(); } }
		private string _TotalFine;
		public string TotalFine { get => _TotalFine; set { _TotalFine = value; OnPropertyChanged(); } }
		public ObservableCollection<BorrowDTO> LastBorrowedBooks
        {
            get => lastBorrowedBooks;
            set
            {
                lastBorrowedBooks = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<FineDTO> RecentFines { get; set; }

        public DashboardLibrarian()
        {
            InitializeComponent();
            this.DataContext = this;
			LoadData();
			ViewFineDetailsCommand = new RelayCommand<FineDTO>(ExecuteViewFineDetails);
            DeleteFineCommand = new RelayCommand<FineDTO>(ExecuteDeleteFine);
            ShowTodayDataCommand = new RelayCommand<object>(ExecuteShowTodayData);
            ShowWeekDataCommand = new RelayCommand<object>(ExecuteShowWeekData);
            ShowMonthDataCommand = new RelayCommand<object>(ExecuteShowMonthData);
            ShowYearDataCommand = new RelayCommand<object>(ExecuteShowYearData);
        }

        private void LoadData()
        {
            using (var db = new LibraryManageSystemContext())
            {
                // Tổng quan thống kê
                // Lấy dữ liệu thực từ cơ sở dữ liệu
                TotalCategory = db.Categories.Count().ToString("N0");
                TotalBook = db.Books.Sum(b => b.Quantity).ToString("N0");
                TotalAccount = db.Users.Count().ToString("N0");
				decimal totalFine = db.Fines.Sum(f => f.FineAmount ?? 0);
				TotalFine = totalFine.ToString("C", new CultureInfo("vi-VN"));

				// Lấy dữ liệu các sách mới mượn gần đây
				LastBorrowedBooks = new ObservableCollection<BorrowDTO>(
                    db.BorrowBooks.Include(b => b.Book).Include(b => b.User)
                    .OrderByDescending(b => b.BorrowDate)
                    .Take(6)
                    .Select(b => new BorrowDTO
                    {
                        Title = b.Book.Title,
                        Desc = b.User.Fullname
                    }).ToList()
                );

                RecentFines = new ObservableCollection<FineDTO>(
                    db.Fines.Include(f => f.Borrow.User)
                    .OrderBy(f => f.PaidDate.HasValue)
                    .ThenByDescending(f => f.FineId)
                    .Select(f => new FineDTO
                    {
                        FineId = f.FineId,
                        Reason = f.FineType,
						FineAmount = f.FineAmount.HasValue ? f.FineAmount.Value.ToString("C", new CultureInfo("vi-VN")) : "0", 
						Paid = f.PaidDate.HasValue ? f.PaidDate.ToString() : "",
                        Status = f.Status == 1 ? "Paid" : "Unpaid",
						DamageFee = f.FineType == "Damage" ? (f.FineAmount ?? 0).ToString("C", new CultureInfo("vi-VN")) : "0",
						OverdueFee = f.FineType == "Overdue" ? (f.FineAmount ?? 0).ToString("C", new CultureInfo("vi-VN")) : "0",
						StudentCode = f.Borrow.User.StudentCode
                    }).ToList()
                );

                dgvFine.ItemsSource = RecentFines;
                totalItems = db.Fines.Count();
                LoadPaginatedData();
            }
        }

        private void LoadPaginatedData()
        {
            using (var db = new LibraryManageSystemContext())
            {
                var fines = db.Fines
                    .Include(f => f.Borrow.User)
                    .OrderBy(f => f.PaidDate.HasValue)
                    .ThenByDescending(f => f.FineId)
                    .Skip((currentPage - 1) * ItemsPerPage)
                    .Take(ItemsPerPage)
                    .Select(f => new FineDTO
                    {
                        FineId = f.FineId,
                        Reason = f.FineType,
						FineAmount = f.FineAmount.HasValue ? f.FineAmount.Value.ToString("C", new CultureInfo("vi-VN")) : "0",
						Paid = f.PaidDate.HasValue ? f.PaidDate.ToString() : "",
						Status = f.Status == 1 ? "Paid" : "Unpaid",
						DamageFee = f.FineType == "Damage" ? (f.FineAmount ?? 0).ToString("C", new CultureInfo("vi-VN")) : "0",
						OverdueFee = f.FineType == "Overdue" ? (f.FineAmount ?? 0).ToString("C", new CultureInfo("vi-VN")) : "0",
						StudentCode = f.Borrow.User.StudentCode
                    }).ToList();

                PaginatedFines = new ObservableCollection<FineDTO>(fines);
                dgvFine.ItemsSource = PaginatedFines;
                UpdatePaginationControls();
            }
        }

        private void UpdatePaginationControls()
        {
            btnPrevious.IsEnabled = currentPage > 1;
            btnNext.IsEnabled = currentPage < TotalPages();
            lblPageInfo.Text = $"Page {currentPage} of {TotalPages()}";
        }

        private int TotalPages()
        {
            return (int)Math.Ceiling((double)totalItems / ItemsPerPage);
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < TotalPages())
            {
                currentPage++;
                LoadPaginatedData();
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPaginatedData();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExecuteViewFineDetails(FineDTO fine)
        {
            if (fine != null)
            {
				decimal fineAmountValue;
				if (decimal.TryParse(fine.FineAmount, out fineAmountValue))
				{
					var dialog = new CustomDialog(
						"Fine Details",
						fine.FineId.ToString(),
						fine.Reason,
						fineAmountValue.ToString("C0", new CultureInfo("vi-VN")), 
						fine.Paid,
						fine.Status,
						fine.StudentCode
					);
					dialog.ShowDialog();
				}
			}
            else
            {
                // Hiển thị CustomDialog khi không có Fine được chọn
                var dialog = new CustomDialog("Information", "Please select a fine to view details.", "", "", "", "", "");
                dialog.ShowDialog();
            }
        }



        private void ExecuteDeleteFine(FineDTO fine)
        {
            if (fine != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete Fine ID {fine.FineId}?",
                                             "Confirm Delete",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (var db = new LibraryManageSystemContext())
                    {
                        var fineToDelete = db.Fines.Find(fine.FineId);
                        if (fineToDelete != null)
                        {
                            db.Fines.Remove(fineToDelete);
                            db.SaveChanges();
                            MessageBox.Show("Fine deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Cập nhật lại danh sách fines
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Fine not found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a fine to delete.", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExecuteShowTodayData(object obj)
        {
            using (var db = new LibraryManageSystemContext())
            {
				DateTime today = DateTime.Today;
				var totalFineToday = db.Fines
					.Where(f => f.PaidDate >= today && f.PaidDate < today.AddDays(1))
					.Sum(f => (decimal?)f.FineAmount) ?? 0;

				string formattedTotalFineToday = totalFineToday.ToString("C", new CultureInfo("vi-VN"));
				OpenCustomDialogShowData("Today's Fines", $"Total fines paid today: {formattedTotalFineToday}");
			}
		}

        private void ExecuteShowWeekData(object obj)
        {
            using (var db = new LibraryManageSystemContext())
            {
                DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var totalFineWeek = db.Fines
                    .Where(f => f.PaidDate >= firstDayOfMonth)
                    .Sum(f => (decimal?)f.FineAmount) ?? 0;
				string formattedTotalFineWeek = totalFineWeek.ToString("C", new CultureInfo("vi-VN"));
				OpenCustomDialogShowData("Monthly Fines", $"Total fines paid this month: {formattedTotalFineWeek}");
            }
        }

        private void ExecuteShowMonthData(object obj)
        {
            using (var db = new LibraryManageSystemContext())
            {
                DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var totalFineMonth = db.Fines
                    .Where(f => f.PaidDate >= firstDayOfMonth)
                    .Sum(f => (decimal?)f.FineAmount) ?? 0;
				string formattedTotalFineMonth = totalFineMonth.ToString("C", new CultureInfo("vi-VN"));
				OpenCustomDialogShowData("Monthly Fines", $"Total fines paid this month: {formattedTotalFineMonth}");
            }
        }

        private void ExecuteShowYearData(object obj)
        {
            using (var db = new LibraryManageSystemContext())
            {
                DateTime firstDayOfYear = new DateTime(DateTime.Now.Year, 1, 1);
                var totalFineYear = db.Fines
                    .Where(f => f.PaidDate >= firstDayOfYear)
                    .Sum(f => (decimal?)f.FineAmount) ?? 0;
				string formattedTotalFineYear = totalFineYear.ToString("C", new CultureInfo("vi-VN"));
				OpenCustomDialogShowData("Yearly Fines", $"Total fines paid this year: {formattedTotalFineYear}");
            }
        }

        private void OpenCustomDialogShowData(string title, string content)
        {
            var dialog = new CustomDialogShowData(title, content);
            dialog.ShowDialog();
        }

        private void OpenCustomDialog(string title, string content)
        {
            var dialog = new CustomDialog(title, content);
            dialog.ShowDialog();
        }

    }
}
