using System;
using System.Collections.Generic;

namespace LibraryManagementWpf.Models
{
    public partial class Thesis
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? FileDoc { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}
