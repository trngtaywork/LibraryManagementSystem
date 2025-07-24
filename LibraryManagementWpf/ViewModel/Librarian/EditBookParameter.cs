using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LibraryManagementWpf.ViewModel.Librarian
{
    public class EditBookParameter
    {
		public string BookId { get; set; }
		public string Title { get; set; }
		public string AuthorNames { get; set; }
		public string Quantity { get; set; }
		public DateTime? PublicationDate { get; set; }
		public BitmapImage SelectedImage { get; set; }
		public string? CategoryId { get; set; }
	}
}
