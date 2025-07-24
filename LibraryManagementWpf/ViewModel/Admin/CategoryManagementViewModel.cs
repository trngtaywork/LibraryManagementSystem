using LibraryManagementWpf.Models;
using LibraryManagementWpf.View.Admin;
using LibraryManagementWpf.ViewModel.Common;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementWpf.ViewModel.Admin
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
		public ICommand DoAddCommand { get; set; }
		public ICommand DeleteCommand { get; set; }
		public ICommand EditCommand { get; set; }
		public ICommand DoEditCommand { get; set; }
		public ICommand CancelCommand { get; set; }
		public ICommand SearchCommand { get; set; }
		public ICommand NextPageCommand { get; }
		public ICommand PreviousPageCommand { get; }

		public ObservableCollection<Models.Category> DisplayedListPerPage { get; private set; }
		private string _DisplayTotalPage;
		public string DisplayTotalPage { get => _DisplayTotalPage; set { _DisplayTotalPage = value; OnPropertyChanged(); } }

		private int _currentPage = 1;
		private int _itemPerPage = 10;
		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				if (_currentPage != value)
				{
					_currentPage = value;
					OnPropertyChanged(nameof(CurrentPage));
					LoadDataCategoryPagination();
				}
			}
		}
		public int TotalPages
		{
			get => (int)Math.Ceiling((double)ListCategory.Count / _itemPerPage);
		}

		public void LoadDataCategoryPagination()
		{
			var skipItems = (CurrentPage - 1) * _itemPerPage;
			DisplayedListPerPage = new ObservableCollection<Models.Category>(ListCategory.Skip(skipItems).Take(_itemPerPage));
			OnPropertyChanged(nameof(DisplayedListPerPage));
			DisplayTotalPage = $"Showing {skipItems} to {skipItems + _itemPerPage} of {TotalPages} entries";
		}
		public bool IsPreviousPageEnabled;
		public bool IsNextPageEnabled;



		public CategoryManagementViewModel() { 
			using (var _context = new LibraryManageSystemContext())
			{
				ListCategory = new ObservableCollection<Category>(_context.Categories);
			}
			var currentUserSession = SessionManager.CurrentUser;
			NextPageCommand = new RelayCommand<object>(
			(_) => CurrentPage < TotalPages,
			(_) =>
			{
				CurrentPage = CurrentPage + 1; if (CurrentPage > 1) { IsPreviousPageEnabled = true; } else { IsPreviousPageEnabled = false; }
			});
			PreviousPageCommand = new RelayCommand<object>(
				(_) => CurrentPage > 1,
				(_) =>
				{
					CurrentPage = CurrentPage - 1; if (CurrentPage < TotalPages) { IsNextPageEnabled = true; } else { IsNextPageEnabled = false; }
				});

			LoadDataCategoryPagination();

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
					LoadDataCategoryPagination();
				}
			});
			AddCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				AddCategory addCategory = new AddCategory();
				addCategory.ShowDialog();
			});
			DoAddCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				if (string.IsNullOrEmpty(CategoryName))
				{
					CategoryNameError = "Không được để trống tên thể loại.";
					return;
				}
				CategoryNameError = "";
				using (var _context = new LibraryManageSystemContext())
				{
					if (_context.Categories.FirstOrDefault(c => c.CategoryName.Equals(CategoryName)) != null)
					{
						MessageBox.Show("Thể loại này đã tồn tại.");
						return;
					}
					Category category = new Category();
					category.CategoryName = CategoryName;
					category.CreatedBy = currentUserSession.Role;
					category.CreatedAt = DateTime.Now;
					_context.Add(category);
					_context.SaveChanges();
					MessageBox.Show("Thêm mới thành công.");
					p.Close();
					CategoryName = "";
					ListCategory = new ObservableCollection<Category>(_context.Categories);
					LoadDataCategoryPagination();
				}
			});
			EditCommand = new RelayCommand<Models.Category>((p) => { return true; }, (p) => {
				var editCategory = new EditCategory();
				var viewModel = new EditCategoryViewModel(p);
				editCategory.DataContext = viewModel;
				editCategory.ShowDialog();
			});
			DoEditCommand = new RelayCommand<EditCategoryParameters>((p) => { return true; }, (p) => {
				if (p != null)
				{
					CategoryName = p.CategoryName;
					CategoryId = p.CategoryId;
					var editCategory = Application.Current.Windows.OfType<EditCategory>().FirstOrDefault();

					if (ValidateAndEdit(editCategory))
					{
						LoadDataCategoryPagination();
						editCategory?.Close();
					}
				}
				
			});
			DeleteCommand = new RelayCommand<Category>((p) => {
				return p != null;
			}, (p) =>
			{
				RemoveCate(p);
				LoadDataCategoryPagination();
			});
			CancelCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
				p.Close();
			});
			SearchCommand = new RelayCommand<object>((p) => { return true; }, (p) => {
				using (var _context = new LibraryManageSystemContext())
				{
					ListCategory = new ObservableCollection<Models.Category>(_context.Categories.Where(u => u.CategoryName.Contains(Keyword)));
					LoadDataCategoryPagination();
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
						ListCategory = new ObservableCollection<Category>(_context.Categories);
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
					ListCategory = new ObservableCollection<Category>(_context.Categories);
					return true;
				}
			}catch (Exception ex)
			{
				MessageBox.Show("Có lỗi khi cập nhật: " + ex.Message);
				return false;
			}
		}
	}
}
