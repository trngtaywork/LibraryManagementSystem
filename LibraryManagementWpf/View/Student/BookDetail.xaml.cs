using LibraryManagementWpf.Models;
using System.Windows;
using System.Windows.Controls;


namespace LibraryManagementWpf.View.Student
{
   
    public partial class BookDetail : UserControl
    {
        public BookDetail( )
        {
            InitializeComponent();
        }
        
        private void ReservationButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Your request to borrow the book has been sent.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
