using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWpf.ViewModel.Admin
{
	public class BorrowBookDetails
	{
		public int BorrowId { get; set; }
		public DateTime? BorrowDate { get; set; }
		public DateTime? DueDate { get; set; }
		public DateTime? ReturnDate { get; set; }
		public int? Status { get; set; }
		public string UserFullName { get; set; }
		public string BookTitle { get; set; }
	}
}
