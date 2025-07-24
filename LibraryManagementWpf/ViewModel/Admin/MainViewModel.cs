using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Admin;
using LibraryManagementWpf.View.Common;
using LibraryManagementWpf.View.Student;
using LibraryManagementWpf.ViewModel.Common;
using LibraryManagementWpf.ViewModel.Student;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static WebView2.DOM.HTMLInputElement;

namespace LibraryManagementWpf.ViewModel.Admin
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
		public IWebView2 WebView { get; set; }
		public ICommand AccountLibViewCommand { get; set; }
		public ICommand AccountStudentViewCommand { get; set; }
		public ICommand DashboardViewCommand { get; set; }
		public ICommand LoginViewCommand { get; set; }
		public ICommand PostViewCommand {  get; set; }
		public ICommand AddPostViewCommand {  get; set; }
		public ICommand AddNewAccountLibrarianViewCommand { get; set; }
		public ICommand AddNewAccountStudentViewCommand { get; set; }
		public ICommand CategoryViewCommand { get; set; }
		public ICommand DocumentaryViewCommand { get; set; }

		public DashboardViewModel dashboardViewModel { get; set; }
		public AccountLibViewModel accountLibManagementViewModel { get; set; }
		public AccountStudentViewModel accountStuManagementViewModel { get; set; }
		public PostViewModel postViewModel { get; set; }
		public AddPostViewModel addPostViewModel {  get; set; }
		public CategoryManagementViewModel categoryManagementViewModel {  get; set; }
		public DocumentaryViewModel documentaryViewModel {  get; set; }

		private object _currentView;
		public object CurrentView
		{
			get { return _currentView; }
			set
			{
				if (_currentView != value)
				{
					_currentView = value;
					OnPropertyChanged(nameof(CurrentView));
				}
			}
		}

		public MainViewModel()
		{
			var currentUserSession = SessionManager.CurrentUser;

			if (currentUserSession != null)
			{
				CurrentUser = currentUserSession;
			}

			dashboardViewModel = new DashboardViewModel();
			accountLibManagementViewModel = new AccountLibViewModel();
			accountStuManagementViewModel = new AccountStudentViewModel();
			postViewModel = new PostViewModel();
			addPostViewModel = new AddPostViewModel();
			categoryManagementViewModel = new CategoryManagementViewModel();
			documentaryViewModel = new DocumentaryViewModel();

			CurrentView = dashboardViewModel;

			CategoryViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				categoryManagementViewModel = new CategoryManagementViewModel();
				CurrentView = categoryManagementViewModel;
			});
			AddNewAccountLibrarianViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				AddNewLibrarianAccount addNewLibrarianAccount = new AddNewLibrarianAccount();
				addNewLibrarianAccount.ShowDialog();
			});
			AddNewAccountStudentViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				AddNewStudentAccount addNewStudentAccount = new AddNewStudentAccount();
				addNewStudentAccount.ShowDialog();
			});
			AddPostViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				addPostViewModel = new AddPostViewModel();
				CurrentView = addPostViewModel;
			});
			PostViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				postViewModel = new PostViewModel();
				CurrentView = postViewModel;
			});
			DashboardViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				dashboardViewModel = new DashboardViewModel();
				CurrentView = dashboardViewModel;
			});
			AccountLibViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				accountLibManagementViewModel = new AccountLibViewModel();
				CurrentView = accountLibManagementViewModel;
			});
			AccountStudentViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				accountStuManagementViewModel = new AccountStudentViewModel();
				CurrentView = accountStuManagementViewModel;
			});
			DocumentaryViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
			{
				documentaryViewModel = new DocumentaryViewModel();
				CurrentView = documentaryViewModel;
			});
			LoginViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				SessionManager.ClearSession();
				p.Close();
			});
		}
	}

	public class AddPostViewModel
	{
	}
}
