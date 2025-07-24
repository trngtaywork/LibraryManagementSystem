using System;
using System.Collections.Generic;

namespace LibraryManagementWpf.Models
{
    public partial class User
    {
        public User()
        {
            BorrowBooks = new HashSet<BorrowBook>();
            Reports = new HashSet<Report>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? StudentCode { get; set; }
        public string? Fullname { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? VerifyCode { get; set; }
        public DateTime? ExpirationCode { get; set; }
        public string? Image {  get; set; }

        public virtual ICollection<BorrowBook> BorrowBooks { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
