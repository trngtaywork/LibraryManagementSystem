using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWpf.DTO
{
    public class FineDTO
    {
        public int FineId { get; set; }
        public int BorrowId { get; set; }
        public string BorrowerName { get; set; } 
        public string FineType { get; set; }
        public string FineAmount { get; set; }
        public string StatusOfFine { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Reason { get; set; }
        public string Paid { get; set; }
        public string Status { get; set; }
        public string DamageFee { get; set; }
        public string OverdueFee { get; set; }
        public string StudentCode { get; set; }
    }

}
