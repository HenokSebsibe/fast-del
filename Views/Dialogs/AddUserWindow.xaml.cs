using System.Windows;

namespace FoodyExpress.Views.Dialogs
{
    public partial class AddUserWindow : Window
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }

        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Username = txtUsername.Text;
            Password = txtPassword.Password;
            var selectedItem = cbRole.SelectedItem as System.Windows.Controls.ComboBoxItem;
            Role = selectedItem?.Content?.ToString() ?? string.Empty;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
