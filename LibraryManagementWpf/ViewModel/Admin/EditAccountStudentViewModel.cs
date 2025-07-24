using LibraryManagementWpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWpf.ViewModel.Admin
{
	public class EditAccountStudentViewModel : BaseViewModel
	{
		private Models.User _user;
		public Models.User User
		{
			get => _user;
			set
			{
				_user = value;
				OnPropertyChanged(nameof(User));
				if (_user != null)
				{
					UserId = _user.UserId.ToString();
					Fullname = _user.Fullname;
					Role = _user.Role;
					Phone = _user.Phone;
					StudentCode = _user.StudentCode;
				}
			}
		}

		private string _UserId;
		public string UserId { get => _UserId; set { _UserId = value; OnPropertyChanged(); } }

		private string _Fullname;
		public string Fullname { get => _Fullname; set { _Fullname = value; OnPropertyChanged(); } }

		private string _Role;
		public string Role { get => _Role; set { _Role = value; OnPropertyChanged(); } }

		private string _Phone;
		public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

		private string _StudentCode;
		public string StudentCode { get => _StudentCode; set { _StudentCode = value; OnPropertyChanged(); } }

		private ObservableCollection<Role> _ListRole;
		public ObservableCollection<Role> ListRole { get => _ListRole; set { _ListRole = value; OnPropertyChanged(); } }


		public EditAccountStudentViewModel(Models.User user)
		{
			User = user;
			LoadRoles();
		}

		private void LoadRoles()
		{
			using (var _context = new LibraryManageSystemContext())
			{
				ListRole = new ObservableCollection<Role>(_context.Roles.Where(r => r.Name == "LIBRARIAN" || r.Name == "STUDENT"));
			}
		}
	}
}

