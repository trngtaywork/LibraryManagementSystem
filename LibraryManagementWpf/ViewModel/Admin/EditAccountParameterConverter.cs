using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LibraryManagementWpf.ViewModel.Admin
{
    class EditAccountParameterConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return new EditAccountParameters
			{
				UserId = values[0]?.ToString(),
				Fullname = values[1]?.ToString(),
				Phone = values[2]?.ToString(),
				Role = values[3]?.ToString(),
				StudentCode = values.Length > 4 ? values[4]?.ToString() : null
			};
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
