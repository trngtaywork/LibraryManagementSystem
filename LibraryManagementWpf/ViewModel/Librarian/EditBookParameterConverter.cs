using LibraryManagementWpf.DTO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace LibraryManagementWpf.ViewModel.Librarian
{
    class EditBookParameterConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return new EditBookParameter
			{
				BookId = values[0]?.ToString(),
				Title = values[1]?.ToString(),
				AuthorNames = values[2]?.ToString(),
				Quantity = values[3]?.ToString(),
				PublicationDate = values[4] as DateTime?,
				SelectedImage = values[5] as BitmapImage, 
				CategoryId = values[6]?.ToString()
			};
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
