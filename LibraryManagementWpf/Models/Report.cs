using System;
using System.Collections.Generic;

namespace LibraryManagementWpf.Models
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public string? ReportReason { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public int? BorrowId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? FineId { get; set; }

        public virtual BorrowBook? Borrow { get; set; }
        public virtual Fine? Fine { get; set; }
        public virtual User? User { get; set; }
    }
}
