using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementWpf.View.Admin
{
	/// <summary>
	/// Interaction logic for Borrow.xaml
	/// </summary>
	public partial class Borrow : UserControl
	{
		public Borrow()
		{
			InitializeComponent();
		}

		public string Title
		{
			get { return (string)GetValue(TitleProperty); } 
			set { SetValue(TitleProperty, value);}
		}
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Borrow));

		public string Desc
		{
			get { return (string)GetValue(DescProperty); }
			set { SetValue(DescProperty, value); }
		}
		public static readonly DependencyProperty DescProperty = DependencyProperty.Register("Desc", typeof(string), typeof(Borrow));

		public FontAwesome.Sharp.IconChar Icon
		{
			get { return (FontAwesome.Sharp.IconChar)GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}
		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(FontAwesome.Sharp.IconChar), typeof(Borrow));
	}
}
