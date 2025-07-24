using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWpf.ViewModel.Admin
{
	public class EditAccountParameters
	{
		public string UserId { get; set; }
		public string Fullname { get; set; }
		public string Phone { get; set; }
		public string Role { get; set; }
		public string? StudentCode { get; set; }

	}
}
