using LibraryManagementWpf.Models;
using LibraryManagementWpf.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace LibraryManagementWpf
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        private CheckerService _fineCheckerService;
        public App()
        {
            var context = new LibraryManageSystemContext();
            var se = new EmailSender.EmailSender();
            _fineCheckerService = new CheckerService(context, se);
        }
    }

}
