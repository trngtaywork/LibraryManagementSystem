using System.Windows.Controls;
using System.Windows;
using static LibraryManagementWpf.View.Student.MyBooks;
using LibraryManagementWpf.Models;
using System.Collections.ObjectModel;

namespace LibraryManagementWpf.View.Student
{
    /// <summary>
    /// Interaction logic for StudentProfile.xaml
    /// </summary>
    public partial class StudentProfile : UserControl
	{
        public ObservableCollection<Post> posts;
        public StudentProfile()
        {
            InitializeComponent();
            LoadNews();
        }
        private void LoadNews()
        {
            using (var _context = new LibraryManageSystemContext())
            {
                posts = new ObservableCollection<Post>(_context.Posts);
				NewsList.ItemsSource = posts;
			}
		}

    }
}
