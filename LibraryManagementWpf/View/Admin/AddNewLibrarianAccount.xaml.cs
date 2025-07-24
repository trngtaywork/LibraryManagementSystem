using System.Windows;
using System.Windows.Input;


namespace LibraryManagementWpf.View.Admin
{
    public partial class AddNewLibrarianAccount : Window
    {
        public AddNewLibrarianAccount()
        {
            InitializeComponent();
        }

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
