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

namespace LibraryManagementWpf.View.Dialogs
{
    /// <summary>
    /// Interaction logic for CustomDialogShowData.xaml
    /// </summary>
    public partial class CustomDialogShowData : Window
    {
        public CustomDialogShowData(string title, string content)
        {
            InitializeComponent();
            TitleText.Text = title;
            ContentText.Text = content;
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
