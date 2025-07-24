using LibraryManagementWpf.Models;
using LibraryManagementWpf.ViewModel.Admin;
using LibraryManagementWpf.ViewModel.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using LibraryManagementWpf.View.Librarian;

namespace LibraryManagementWpf.ViewModel.Librarian
{
	public class CategoryManagementViewModel : BaseViewModel
	{

		private ObservableCollection<Category> _ListCategory;
		public ObservableCollection<Category> ListCategory { get => _ListCategory; set { _ListCategory = value; OnPropertyChanged(); } }
		private string _CategoryName;
		public string CategoryName { get => _CategoryName; set { _CategoryName = value; OnPropertyChanged(); } }
		private string _CategoryId;
		public string CategoryId { get => _CategoryId; set { _CategoryId = value; OnPropertyChanged(); } }

		private string _CategoryNameError;
		public string CategoryNameError { get => _CategoryNameError; set { _CategoryNameError = value; OnPropertyChanged(); } }

		private string _selectedFilterOption = "All";
		public string SelectedFilterOption
		{
			get => _selectedFilterOption;
			set
			{
				if (_selectedFilterOption != value)
				{
					_selectedFilterOption = value;
					OnPropertyChanged(nameof(SelectedFilterOption));
					FilterCommand.Execute(null);
				}
			}
		}
		private string _Keyword;
		public string Keyword { get => _Keyword; set { _Keyword = value; OnPropertyChanged(); } }
		public ICommand FilterCommand { get; set; }
		public ICommand AddCommand { get; set; }
		public ICommand DoAddCateLibCommand { get; set; }
		public ICommand DeleteCommand { get; set; }
		public ICommand EditCommand { get; set; }
		public ICommand DoEditCateLibCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public ICommand SearchCommand { get; set; }

		public void LoadDataCategory()
		{
			using (var _context = new LibraryManageSystemContext())
			{
				ListCategory = new ObservableCollection<Category>(_context.Categories);
			}
		}
		public CategoryManagementViewModel()
		{
			LoadDataCategory();

			var currentUserSession = SessionManager.CurrentUser;

			FilterCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				using (var _context = new LibraryManageSystemContext())
				{
					IQueryable<Models.Category> query = _context.Categories;

					query = SelectedFilterOption switch
					{
						"Tên" => query.OrderBy(u => u.CategoryName),
						"Id" => query.OrderBy(u => u.CategoryId),
						"All" => query.OrderBy(u => u.CreatedAt)
					};

					ListCategory = new ObservableCollection<Models.Category>(query.ToList());
				}
			});
			AddCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				AddCategory addCategory = new AddCategory();
				addCategory.ShowDialog();
			});
			DoAddCateLibCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				if (string.IsNullOrEmpty(CategoryName))
				{
					CategoryNameError = "Không được để trống tên thể loại.";
					return;
				}
				CategoryNameError = "";
				using (var _context = new LibraryManageSystemContext())
				{
					Category category = new Category();
					category.CategoryName = CategoryName;
					category.CreatedBy = currentUserSession.Fullname;
					category.CreatedAt = DateTime.Now;
					_context.Add(category);
					_context.SaveChanges();
					MessageBox.Show("Thêm mới thành công.");
					p.Close();
					CategoryName = "";
					LoadDataCategory();
				}
			});
			EditCommand = new RelayCommand<Models.Category>((p) => { return true; }, (p) => {
				var editCategory = new EditCategory();
				var viewModel = new EditCategoryViewModel(p);
				editCategory.DataContext = viewModel;
				editCategory.ShowDialog();
			});
			DoEditCateLibCommand = new RelayCommand<EditCategoryParameters>((p) => { return true; }, (p) => {
				if (p != null)
				{
					CategoryName = p.CategoryName;
					CategoryId = p.CategoryId;
					var editCategory = Application.Current.Windows.OfType<EditCategory>().FirstOrDefault();

					if (ValidateAndEdit(editCategory))
					{
						editCategory?.Close();
					}
				}

			});
			DeleteCommand = new RelayCommand<Category>((p) => {
				return p != null;
			}, (p) =>
			{
				RemoveCate(p);
			});
			CancelCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				p.Close();
			});
			SearchCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				using (var _context = new LibraryManageSystemContext())
				{
					ListCategory = new ObservableCollection<Models.Category>(_context.Categories.Where(u => u.CategoryName.Contains(Keyword)));
				}
			});
		}
		public void RemoveCate(Models.Category p)
		{
			var result = MessageBox.Show(
					$"Are you sure you want to delete category with id '{p.CategoryId}' ?",
					"Delete User",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question
				);
			if (result == MessageBoxResult.Yes)
			{
				using (LibraryManageSystemContext _context = new LibraryManageSystemContext())
				{
					try
					{
						var cateWithBooks = _context.Categories
							.Where(c => _context.Books.Any(b => b.CategoryId == c.CategoryId))
							.FirstOrDefault(u => u.CategoryId == p.CategoryId);
						if (cateWithBooks == null)
						{
							Category cateToRemove = _context.Categories.FirstOrDefault(c => c.CategoryId == p.CategoryId);
							_context.Remove(cateToRemove);
							_context.SaveChangesAsync();
							MessageBox.Show("Xóa thể loại thành công !");
						}
						else
						{
							MessageBox.Show("Đã có sách thuộc thể loại này !");
						}
						LoadDataCategory();
					}
					catch (Exception ex)
					{
						MessageBox.Show("Xóa không thành công !");
					}
				}
			}
		}

		public bool ValidateAndEdit(EditCategory editCategory)
		{
			if (string.IsNullOrEmpty(CategoryName))
			{
				MessageBox.Show("Không được để trống tên thể loại.");
				return false;
			}
			try
			{
				using (var _context = new LibraryManageSystemContext())
				{
					Category category = _context.Categories.FirstOrDefault(u => u.CategoryId == int.Parse(CategoryId));
					category.CategoryName = CategoryName;
					_context.Update(category);
					_context.SaveChanges();
					MessageBox.Show("Cập nhật thành công.");
					CategoryName = "";
					LoadDataCategory();
					return true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi cập nhật: " + ex.Message);
				return false;
			}
		}
	}
}

