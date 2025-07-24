using LibraryManagementWpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWpf.ViewModel.Librarian
{
	public class EditCategoryViewModel : BaseViewModel
	{
		private string _CategoryName;
		public string CategoryName { get => _CategoryName; set { _CategoryName = value; OnPropertyChanged(); } }
		private string _CategoryId;
		public string CategoryId { get => _CategoryId; set { _CategoryId = value; OnPropertyChanged(); } }
		public EditCategoryViewModel(Category category)
		{
			CategoryName = category.CategoryName;
			CategoryId = category.CategoryId.ToString();
		}
	}
}
