using LibraryManagementWpf.ViewModel.Librarian;
using LibraryManagementWpf.ViewModel.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static LibraryManagementWpf.View.Librarian.BorrowManagement;

namespace LibraryManagementWpf.View.Librarian
{
    /// <summary>
    /// Interaction logic for IssuedHistory.xaml
    /// </summary>
    public partial class IssuedHistory : UserControl
    {

        public IssuedHistory()
        {
            InitializeComponent();
        }

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (DataContext is IssuedHistoryViewModel issuedHistory)
			{
				issuedHistory.LoadBorrowBooks();
			}
		}
	}
}
