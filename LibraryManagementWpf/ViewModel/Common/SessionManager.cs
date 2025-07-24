using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWpf.ViewModel.Common
{
	public static class SessionManager
	{
		public static User CurrentUser { get; set; }

		public static void ClearSession()
		{
			Authen authen = new Authen();
			authen.DataContext = new AuthenViewModel();
			authen.Show();
			CurrentUser = null;
		}
	}

	
}
