using LibraryManagementWpf.DTO;
using LibraryManagementWpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LibraryManagementWpf.ViewModel.Librarian
{
    public class EditBookViewModel : BaseViewModel
    {
		private DateTime? _publicDate;

		public string BookId { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string Quantity { get; set; }
		public DateTime? PublicDate
		{
			get => _publicDate;
			set
			{
				_publicDate = value;
				OnPropertyChanged(nameof(PublicDate));
			}
		}
		public int? CategoryId { get; set; }
		public BitmapImage SelectedImage { get; set; }

		private ObservableCollection<Category> _ListCategory;
		public ObservableCollection<Category> ListCategory { get => _ListCategory; set { _ListCategory = value; OnPropertyChanged(); } }

		public EditBookViewModel(BookDTO book)
		{
			using (var _context = new LibraryManageSystemContext())
			{
				ListCategory = new ObservableCollection<Category>(_context.Categories);
			}
			BookId = book.BookId.ToString();
			Title = book.Title;
			Author = string.Join(", ", book.AuthorNames);
			Quantity = book.Quantity.ToString();
			PublicDate = book.PublicationDate; 
			if (book.Image != null)
			{
				SelectedImage = new BitmapImage(new Uri(book.Image, UriKind.RelativeOrAbsolute));
			}
			CategoryId = book.CategoryId;
		}
	}
}
