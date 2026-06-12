using System.Windows;
using Microsoft.Data.Sqlite;
using FoodyExpress.Database;
using FoodyExpress.Views.Dialogs;
using System.Security.Cryptography;
using System.Text;

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

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AddUserWindow();
            if (dlg.ShowDialog() == true)
            {
                string? username = dlg.Username;
                string? password = dlg.Password;
                string? role = dlg.Role;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
                {
                    ShowError("All fields are required to register.");
                    return;
                }

                try
                {
                    using (var connection = DatabaseHelper.GetConnection())
                    {
                        var insert = connection.CreateCommand();
                        insert.CommandText = "INSERT INTO Users (Username, PasswordHash, RoleId) VALUES (@u, @p, (SELECT Id FROM Roles WHERE Name = @r))";
                        insert.Parameters.AddWithValue("@u", username);
                        insert.Parameters.AddWithValue("@p", ComputeHash(password));
                        insert.Parameters.AddWithValue("@r", role);
                        insert.ExecuteNonQuery();
                        
                        MessageBox.Show("Registration successful. Please login.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtError.Visibility = Visibility.Collapsed;
                    }
                }
                catch (System.Exception ex)
                {
                    ShowError($"Registration error: {ex.Message}");
                }
            }
        }

        private static string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return System.Convert.ToBase64String(hash);
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
