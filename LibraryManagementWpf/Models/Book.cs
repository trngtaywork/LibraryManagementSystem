using System;
using System.Collections.Generic;

namespace LibraryManagementWpf.Models
{
    public partial class Book
    {
        public Book()
        {
            BorrowBooks = new HashSet<BorrowBook>();
            Authors = new HashSet<Author>();
        }

        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<BorrowBook> BorrowBooks { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
    }
}
