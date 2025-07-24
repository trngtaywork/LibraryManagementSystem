using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Admin;
using LibraryManagementWpf.View.Librarian;
using LibraryManagementWpf.ViewModel.Admin;
using Microsoft.EntityFrameworkCore;

using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Input;

namespace LibraryManagementWpf.ViewModel.Librarian
{
	public class ReportLibViewModel : BaseViewModel
	{
		private ObservableCollection<Models.Report> _Reports;
		public ObservableCollection<Models.Report> Reports { get => _Reports; set { _Reports = value; OnPropertyChanged(); } }
		private string _Keyword;
		public string Keyword { get => _Keyword; set { _Keyword = value; OnPropertyChanged(); } }
		public ICommand ReportViewDetailsCommand { get; set; }
		public ICommand EditReportCommand { get; set; }
		public ICommand FilterCommand { get; set; }
		public ICommand SearchCommand { get; set; }
		public ICommand CancelReportCommand { get; set; }

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

		public void LoadDataReport()
		{
			using (var _context = new LibraryManageSystemContext())
			{
				Reports = new ObservableCollection<Models.Report>(_context.Reports.Include(r => r.User));
			}
		}

		public ReportLibViewModel()
		{
			LoadDataReport();

			ReportViewDetailsCommand = new RelayCommand<Models.Report>((p) => { return true; }, (p) => {
				var report = new ReportDetails();
				var viewModel = new ViewReportDetailsViewModel(p);
				report.DataContext = viewModel;
				report.ShowDialog();
			});
			CancelReportCommand = new RelayCommand<Models.Report>((p) => { return true; }, (p) => {
				MessageBoxResult result = MessageBox.Show(
						"Bạn có muốn tiếp tục?",
						"Confirmation",
						MessageBoxButton.YesNo,
						MessageBoxImage.Question
					);
				if (result == MessageBoxResult.Yes)
				{
					try
					{
						using (var _context = new LibraryManageSystemContext())
						{
							Models.Report report = p;
							report.Status = "reject";
							_context.Reports.Update(report);
							_context.SaveChanges();
							LoadDataReport();
							MessageBox.Show("Cập nhật trạng thái report thành công.");
						}
					}
					catch { MessageBox.Show("Lỗi khi cập nhật trạng thái report."); }
				}
			});
			EditReportCommand = new RelayCommand<Models.Report>((p) => { return true; }, (p) => {
				MessageBoxResult result = MessageBox.Show(
						"Bạn có muốn tiếp tục?",       
						"Confirmation",                   
						MessageBoxButton.YesNo,            
						MessageBoxImage.Question          
					);
				if (result == MessageBoxResult.Yes)
				{
					try
					{
						using (var _context = new LibraryManageSystemContext())
						{
							Models.Report report = p;
							report.Status = "done";
							_context.Reports.Update(report);
							_context.SaveChanges();
							LoadDataReport();
							MessageBox.Show("Cập nhật trạng thái report thành công.");
						}
					}
					catch { MessageBox.Show("Lỗi khi cập nhật trạng thái report."); }
				}
			});
			FilterCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				using (var _context = new LibraryManageSystemContext())
				{
					IQueryable<Models.Report> query = null;
					query = SelectedFilterOption switch
					{
						"Done" => _context.Reports.Where(r => r.Status == "done"),
						"In process" => _context.Reports.Where(r=>r.Status == "processing"),
						"All" => _context.Reports
					};

					Reports = new ObservableCollection<Models.Report>(query.ToList());
				}
			});
			SearchCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				using (var _context = new LibraryManageSystemContext())
				{
					Reports = new ObservableCollection<Models.Report>(_context.Reports.Include(r => r.User).Where(r => r.User.StudentCode.Contains(Keyword) || r.User.Fullname.Contains(Keyword) || r.User.Email.Contains(Keyword)));
				}
			});
		}
	}
}
