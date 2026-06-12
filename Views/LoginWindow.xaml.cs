using System.Windows;
using Microsoft.Data.Sqlite;
using FoodyExpress.Database;

namespace FoodyExpress.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Please enter username and password.");
                return;
            }

            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    string query = "SELECT UserID, Role FROM Users WHERE Username = @Username AND PasswordHash = @Password";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string role = reader.GetString(1);

                                MainWindow main = new MainWindow(userId, role);
                                main.Show();
                                this.Close();
                            }
                            else
                            {
                                ShowError("Invalid username or password.");
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ShowError($"Database error: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
