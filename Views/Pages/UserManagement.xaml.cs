using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;
using FoodyExpress.Views.Dialogs;

namespace FoodyExpress.Views.Pages
{
    public partial class UserManagement : UserControl
    {
        private ObservableCollection<FoodyExpress.Models.User> _users = new();

        public UserManagement()
        {
            InitializeComponent();
            LoadUsers();
        }
        private void LoadUsers()
        {
            _users.Clear();
            using var conn = Database.DatabaseHelper.GetConnection();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT UserID, Username, Role FROM Users";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _users.Add(new FoodyExpress.Models.User
                {
                    UserID = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Role = reader.GetString(2)
                });
            }
            dgUsers.ItemsSource = _users;
        }
        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add User dialog not implemented. Implement a dialog to collect Username, Password, Role and insert into Users table.");
                        var dlg = new AddUserWindow();
            if (dlg.ShowDialog() == true)
            {
                string? username = dlg.Username;
                string? password = dlg.Password;
                string? role = dlg.Role;
                using var conn = Database.DatabaseHelper.GetConnection();
                var insert = conn.CreateCommand();
                insert.CommandText = "INSERT INTO Users (Username, PasswordHash, RoleId) VALUES (@u, @p, (SELECT Id FROM Roles WHERE Name = @r))";
                insert.Parameters.AddWithValue("@u", username);
                insert.Parameters.AddWithValue("@p", ComputeHash(password ?? ""));
                insert.Parameters.AddWithValue("@r", role);
                insert.ExecuteNonQuery();
                LoadUsers();
            }
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem is FoodyExpress.Models.User selected)
            {
                MessageBox.Show($"Edit User {{selected.Username}} not implemented. Implement edit dialog and UPDATE statement.");
            }
            else
            {
                MessageBox.Show("Select a user to edit.");
            }
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem is FoodyExpress.Models.User selected)
            {
                var result = MessageBox.Show($"Delete user {{selected.Username}}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using var conn = Database.DatabaseHelper.GetConnection();
                    var del = conn.CreateCommand();
                    del.CommandText = "DELETE FROM Users WHERE UserID = @id";
                    del.Parameters.AddWithValue("@id", selected.UserID);
                    del.ExecuteNonQuery();
                    LoadUsers();
                }
            }
            else
            {
                MessageBox.Show("Select a user to delete.");
            }
        }
            private static string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
