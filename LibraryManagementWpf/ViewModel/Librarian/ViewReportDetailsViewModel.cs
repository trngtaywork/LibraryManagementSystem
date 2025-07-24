using LibraryManagementWpf.Models;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementWpf.ViewModel.Librarian
{
	public class ViewReportDetailsViewModel : BaseViewModel
	{
		private Models.Report _Report;
		public Models.Report Report
		{
			get => _Report;
			set
			{
				_Report = value;
				OnPropertyChanged(nameof(Report));
			}
		}
		public ViewReportDetailsViewModel(Report report) {
			Report = report;
		}	
	}
}
