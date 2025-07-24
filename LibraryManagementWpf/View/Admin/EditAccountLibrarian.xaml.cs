using LibraryManagementWpf.Models;
using LibraryManagementWpf.ViewModel.Admin;
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

namespace LibraryManagementWpf.View.Admin
{
    /// <summary>
    /// Interaction logic for EditAccountLibrarian.xaml
    /// </summary>
    public partial class EditAccountLibrarian : Window
    {
        public EditAccountLibrarian()
        {
            InitializeComponent();
        }


		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}
	}
}
