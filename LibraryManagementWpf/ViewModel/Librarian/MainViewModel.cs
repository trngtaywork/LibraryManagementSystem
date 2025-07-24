using LibraryManagementWpf.View.Admin;
using LibraryManagementWpf.View.Common;
using LibraryManagementWpf.View.Librarian;
using LibraryManagementWpf.ViewModel.Admin;
using LibraryManagementWpf.ViewModel.Common;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementWpf.ViewModel.Librarian
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
		public ICommand BookViewCommand { get; set; }
		public ICommand BorrowViewCommand{ get; set; }
		public ICommand IssuedHistoryViewCommand { get; set; }
		public ICommand DashboardViewCommand { get; set; }
		public ICommand FineManagementViewCommand { get; set; }
		public ICommand LoginViewCommand { get; set; }
		public ICommand ReportViewCommand { get; set; }
		public ICommand CategoryViewCommand { get; set; }

		public DashboardLibViewModel dashboardLibViewModel { get; set; }
		public BookManagementViewModel bookManagementViewModel { get; set; }
		public BorrowManagementViewModel borrowManagementViewModel { get; set; }
		public IssuedHistoryViewModel issuedHistoryViewModel { get; set; }
		public FineManagementViewModel fineManagementViewModel { get; set; }
		public ReportLibViewModel reportLibViewModel {  get; set; }
		public CategoryManagementViewModel categoryManagementViewModel { get; set; }

		private object _currentView;
		public object CurrentView
		{
			get { return _currentView; }
			set { _currentView = value; OnPropertyChanged(); }
		}
		public MainViewModel()
		{
			var currentUserSession = SessionManager.CurrentUser;

			if (currentUserSession != null)
			{
				CurrentUser = currentUserSession;
			}
			dashboardLibViewModel = new DashboardLibViewModel(this);
			bookManagementViewModel = new BookManagementViewModel();
			borrowManagementViewModel = new BorrowManagementViewModel();
			issuedHistoryViewModel = new IssuedHistoryViewModel();
			fineManagementViewModel = new FineManagementViewModel();
			categoryManagementViewModel = new CategoryManagementViewModel();
			reportLibViewModel = new ReportLibViewModel();
			CurrentView = dashboardLibViewModel;

			DashboardViewCommand = new RelayCommand<Object>((p) => { return true; }, (p) => {
				CurrentView = dashboardLibViewModel;
			});
			CategoryViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				categoryManagementViewModel = new CategoryManagementViewModel();
				CurrentView = categoryManagementViewModel;
			});
			BookViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				CurrentView = bookManagementViewModel;
			});

			BorrowViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				CurrentView = borrowManagementViewModel;
			});
			IssuedHistoryViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				issuedHistoryViewModel = new IssuedHistoryViewModel();
				CurrentView = issuedHistoryViewModel;
			});
			FineManagementViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				CurrentView = fineManagementViewModel;
			});
			ReportViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				CurrentView = reportLibViewModel;
			});
		
			LoginViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				SessionManager.ClearSession();
				p.Close();
			});
		}
	}

	public class DashboardLibViewModel
	{
		private MainViewModel _libMainViewModel;
		public DashboardLibViewModel(MainViewModel libMainViewModel)
		{
			_libMainViewModel = libMainViewModel;
		}
	}

	public class FineManagementViewModel
	{
	}

}

