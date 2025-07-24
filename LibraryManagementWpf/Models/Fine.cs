using System;
using System.Collections.Generic;

namespace LibraryManagementWpf.Models
{
    public partial class Fine
    {
        public Fine()
        {
            Reports = new HashSet<Report>();
        }

        public int FineId { get; set; }
        public string? FineType { get; set; }
        public decimal? FineAmount { get; set; }
        public int? Status { get; set; }
        public DateTime? PaidDate { get; set; }
        public int? BorrowId { get; set; }

        public virtual BorrowBook? Borrow { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
