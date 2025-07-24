using System.Windows;

namespace LibraryManagementWpf.View.Dialogs
{
    public partial class CustomDialog : Window
    {
        public CustomDialog(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public CustomDialog(string title, string fineId, string reason, string amount, string paid, string status, string studentCode)
        {
            InitializeComponent();
            TitleText.Text = title;
            FineIdText.Text = fineId;
            ReasonText.Text = reason;
            AmountText.Text = amount;
            PaidText.Text = paid;
            StatusText.Text = status;
            StudentCodeText.Text = studentCode;
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}