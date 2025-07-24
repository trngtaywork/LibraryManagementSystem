using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Admin;
using LibraryManagementWpf.View.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static WebView2.DOM.HTMLInputElement;

namespace LibraryManagementWpf.ViewModel.Admin
{
	public class AccountLibViewModel : BaseViewModel
	{
		private string _UserId;
		public string UserId { get => _UserId; set { _UserId = value; OnPropertyChanged(); } }

		private string _Fullname;
		public string Fullname { get => _Fullname; set { _Fullname = value; OnPropertyChanged(); } }

		private string _FullnameError;
		public string FullnameError { get => _FullnameError; set { _FullnameError = value; OnPropertyChanged(); } }

		private string _Role;
		public string Role { get => _Role; set { _Role = value; OnPropertyChanged(); } }

		private string _Email;
		public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

		private string _EmailError;
		public string EmailError { get => _EmailError; set { _EmailError = value; OnPropertyChanged(); } }

		private string _Phone;
		public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

		private string _PhoneError;
		public string PhoneError { get => _PhoneError; set { _PhoneError = value; OnPropertyChanged(); } }

		private string _DisplayTotalPage;
		public string DisplayTotalPage { get => _DisplayTotalPage; set { _DisplayTotalPage = value; OnPropertyChanged(); } }
		private string _Keyword;
		public string Keyword { get => _Keyword; set { _Keyword = value; OnPropertyChanged(); } }
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

		private ObservableCollection<Role> _ListRole;
		public ObservableCollection<Role> ListRole { get => _ListRole; set { _ListRole = value; OnPropertyChanged(); } }

		public ICommand DoEditCommand { get; set; }
		public ICommand AddCommand { get; set; }
		public ICommand RemoveCommand { get; set; }
		public ICommand EditCommand { get; set; }
		public ICommand CancelEditAccountLibrarianViewCommand { get; set; }
		public ICommand SearchCommand { get; set; }
		public ICommand FilterCommand { get; set; }


		private ObservableCollection<Models.User> _ListLibrarian;
		public ObservableCollection<Models.User> ListLibrarian { get => _ListLibrarian; set { _ListLibrarian = value; OnPropertyChanged(); } }
		public ObservableCollection<Models.User> DisplayedListPerPage { get; private set; }

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
					LoadLibAcc();
				}
			}
		}
		public int TotalPages {
			get => (int)Math.Ceiling((double)ListLibrarian.Count / _itemPerPage);
		}

		public ICommand NextPageCommand { get; }
		public ICommand PreviousPageCommand { get; }
		
		public void LoadLibAcc()
		{
			var skipItems = (CurrentPage - 1) * _itemPerPage;
			DisplayedListPerPage = new ObservableCollection<Models.User>(ListLibrarian.Skip(skipItems).Take(_itemPerPage));
			OnPropertyChanged(nameof(DisplayedListPerPage));
			DisplayTotalPage = $"Showing {skipItems} to {skipItems + _itemPerPage} of {TotalPages} entries";
		}

		public bool IsPreviousPageEnabled;
		public bool IsNextPageEnabled;
		public AccountLibViewModel()
		{
			using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
			{
				ListLibrarian = new ObservableCollection<Models.User>(_context.Users.Where(u => u.Role == "LIBRARIAN" && u.Status == 1).OrderBy(u=>u.CreateAt).ToList());
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


			LoadLibAcc();

			CancelEditAccountLibrarianViewCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
			{
				p.Close();
			});

			AddCommand = new RelayCommand<Window>((p) =>
			{
				return true;
			}, (p) =>
			{
				AddNewLibrarianAccount(p);

			});

			RemoveCommand = new RelayCommand<Models.User>((p) =>
			{
				return p != null;
			}, (p) =>
			{
				RemoveLibAccount(p);
			});

			EditCommand = new RelayCommand<Models.User>((p) => { return true; }, (p) => {
				var editAccountLibrarian = new EditAccountLibrarian();
				var viewModel = new EditAccountLibrarianViewModel(p);
				editAccountLibrarian.DataContext = viewModel;
				editAccountLibrarian.ShowDialog();
			});

			DoEditCommand = new RelayCommand<EditAccountParameters>((p) => true, (p) =>
			{
				if (p != null)
				{
					UserId = p.UserId;
					Fullname = p.Fullname;
					Phone = p.Phone;
					Role = p.Role;
					var editAccountLibrarian = Application.Current.Windows.OfType<EditAccountLibrarian>().FirstOrDefault();

					if (ValidateAndEdit(editAccountLibrarian))
					{
						editAccountLibrarian?.Close();
					}
				}
			});

			SearchCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				using (var _context = new LibraryManageSystemContext())
				{
					ListLibrarian = new ObservableCollection<Models.User>(_context.Users.Where(u => u.Role == "Librarian" || u.Fullname.Contains(Keyword) || u.Email.Contains(Keyword)));
					CurrentPage = 1;
					LoadLibAcc();
				}
			});

			FilterCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				using (var _context = new LibraryManageSystemContext())
				{
					IQueryable<Models.User> query = _context.Users.Where(u => u.Role == "Librarian");

					query = SelectedFilterOption switch
					{
						"Tên" => query.OrderBy(u => u.Fullname),
						"Email" => query.OrderBy(u => u.Email), 
						"All" => query.OrderBy(u => u.CreateAt)
					};

					ListLibrarian = new ObservableCollection<Models.User>(query.ToList());
					LoadLibAcc();
				}
			});
		}

		public void RemoveLibAccount(Models.User p)
		{
			var result = MessageBox.Show(
					$"Are you sure you want to delete user with id '{p.UserId}' ?",
					"Delete User",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question
				);
			if (result == MessageBoxResult.Yes)
			{
				using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
				{
					try
					{
						var userInDb = _context.Users.FirstOrDefault(u => u.UserId == p.UserId);
						if (userInDb != null)
						{
							userInDb.Status = 0;
							_context.Update(userInDb);
							_context.SaveChangesAsync();
						}
						MessageBox.Show("Xóa tài khoản thành công !");
						ListLibrarian = new ObservableCollection<Models.User>(_context.Users.Where(u => u.Role == "LIBRARIAN" && u.Status == 1).ToList());
						CurrentPage = 1;
						LoadLibAcc();
					}
					catch (Exception ex)
					{
						MessageBox.Show("Xóa tài khoản không thành công !");
					}
				}
			}
		}

		public bool IsValidEmail(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		private async void AddNewLibrarianAccount(Window p)
		{
			if (string.IsNullOrEmpty(Phone) && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Fullname))
			{
				PhoneError = "Không được để trống các trường.";
				return ;
			}
			else if (string.IsNullOrEmpty(Fullname))
			{
				EmailError = "";
				PhoneError = "";
				Fullname = "Không được để trống họ và tên.";
				return ;
			}
			else if (string.IsNullOrEmpty(Email))
			{
				PhoneError = "";
				FullnameError = "";
				EmailError = "Không được để trống email.";
				return ;
			}else if (!IsValidEmail(Email))
			{
				PhoneError = "";
				FullnameError = "";
				EmailError = "Email định dạng không hợp lệ !";
				return;
			}
			else if (string.IsNullOrEmpty(Phone))
			{
				EmailError = "";
				FullnameError = "";
				PhoneError = "Không được để trống số điện thoại.";
				return ;
			}
			else if (!Regex.IsMatch(Phone, @"^\d{10}$"))
			{
				EmailError = "";
				FullnameError = "";
				PhoneError = "Số điện thoại phải có 10 chữ số.";
				return;
			}
			EmailError = "";
			PhoneError = "";
			FullnameError = "";
			try
			{
				using (var _context = new LibraryManageSystemContext())
				{
					if(_context.Users.SingleOrDefault(u=>u.Email == Email)!=null)
					{
						MessageBox.Show("Tài khoản với email: " + Email + " đã tồn tại.");
						return;
					}
					Models.User user = new Models.User();
					user.Email = Email.Trim();
					user.Fullname = Fullname.Trim();
					user.Phone = Phone.Trim();
					user.Status = 1;
					user.Role = "LIBRARIAN";
					user.CreateAt = DateTime.Now;
					_context.Add(user);
					_context.SaveChanges();
					ListLibrarian = new ObservableCollection<Models.User>(_context.Users.Where(u => u.Role == "LIBRARIAN" && u.Status == 1).ToList());
					CurrentPage = 1;
					LoadLibAcc();

					MessageBox.Show("Thêm mới thành công.");
					p.Close();
					if (_context.Users.SingleOrDefault(u => u.Email == Email) != null)
					{
						var em = new EmailSender.EmailSender();
						await em.SendEmail(Email.Trim(), "Library Management Reset Password", "resetPasswordCode");
					}
					Phone = "";
					Fullname = "";
					Email = "";

				}
			}catch(Exception ex)
			{
				MessageBox.Show("Có lỗi khi thêm mới: " + ex.Message);
			}
		}

		private bool ValidateAndEdit(EditAccountLibrarian editAccountLibrarian)
		{

			if (string.IsNullOrEmpty(Phone) && string.IsNullOrEmpty(Role) && string.IsNullOrEmpty(Fullname))
			{
				MessageBox.Show("Không được để trống các trường.");
				return false;
			}
			else if (string.IsNullOrEmpty(Fullname))
			{
				MessageBox.Show("Không được để trống họ và tên.");
				return false;
			}
			else if (string.IsNullOrEmpty(Role))
			{
				MessageBox.Show("Không được để trống vai trò.");
				return false;
			}
			else if (string.IsNullOrEmpty(Phone))
			{
				MessageBox.Show("Không được để trống số điện thoại.");
				return false;
			}
			else if (!Regex.IsMatch(Phone, @"^\d{10}$"))
			{
				MessageBox.Show("Số điện thoại phải có 10 chữ số.");
				return false;
			}
			try
			{
				using (var _context = new LibraryManageSystemContext())
				{
					Models.User user = _context.Users.FirstOrDefault(u => u.UserId == int.Parse(UserId));
					if (user != null)
					{
						user.Phone = Phone;
						user.Role = Role;
						user.Fullname = Fullname;

						_context.Update(user);
						_context.SaveChanges();
						ListLibrarian = new ObservableCollection<Models.User>(_context.Users.Where(u => u.Role == "LIBRARIAN" && u.Status == 1).ToList());
						CurrentPage = 1;
						LoadLibAcc();
						MessageBox.Show("Cập nhật thành công.");

						Phone = "";
						Role = "";
						Fullname = "";
						UserId = "";
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi cập nhật: " + ex.Message);
			}
			return false;
		}
	}
	
}
