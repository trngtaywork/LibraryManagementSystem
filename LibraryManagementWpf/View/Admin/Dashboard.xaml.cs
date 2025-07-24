using LibraryManagementWpf.ViewModel.Admin;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace LibraryManagementWpf.View.Admin
{
	/// <summary>
	/// Interaction logic for Dashboard.xaml
	/// </summary>
	public partial class Dashboard : UserControl
	{
		public Dashboard()
		{
			InitializeComponent();
			DataContext = new DashboardViewModel(); 
		}
	}
}
