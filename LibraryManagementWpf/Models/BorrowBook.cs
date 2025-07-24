using System;
using System.Collections.Generic;

namespace LibraryManagementWpf.Models
{
    public partial class BorrowBook
    {
        public BorrowBook()
        {
            Fines = new HashSet<Fine>();
            Reports = new HashSet<Report>();
        }

        public int BorrowId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? LibrarianInCharge { get; set; }
        public int? Status { get; set; }
        public int? BookId { get; set; }
        public int? UserId { get; set; }

        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Fine> Fines { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
		public string StatusString
		{
			get
			{
                return Status switch
                {
                    0 => "Reservation",
                    1 => "Borrowing",
                    5 => "Return on time",
                    4 => "Lost/Not returned",
                    3 => "Overdue",
                    2 => "Cancel",
                    _ => ""
                };
            }
		}
	}
}
