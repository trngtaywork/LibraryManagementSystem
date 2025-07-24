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

namespace LibraryManagementWpf.View.Common
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Authen : Window
    {
        public Authen()
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
		private bool isMaximize = false;
		private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				if (isMaximize)
				{
					this.WindowState = WindowState.Normal;
					this.Width = 1080;
					this.Height = 720;
					isMaximize = false;
				}
				else
				{
					this.WindowState = WindowState.Maximized;
					isMaximize = true;
				}
			}
		}
	}
}
