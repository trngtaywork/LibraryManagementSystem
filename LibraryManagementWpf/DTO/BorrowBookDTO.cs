using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWpf.DTO
{
    public class BorrowBookDTO
    {
        public int BorrowId { get; set; }
        public string StudentCode { get; set; }
        public string BorrowerName { get; set; }
        public string BookTitle { get; set; }
        public DateTime? ReservationDate { get; set; } 
        public DateTime? BorrowDate { get; set; }
        public DateTime? DueDate { get; set; }
		public DateTime? ReturnDate { get; set; }
        public string? LibrarianInCharge { get; set; }
        public BorrowStatus Status { get; set; }
    }
}
