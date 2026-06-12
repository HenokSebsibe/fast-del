using System;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using FoodyExpress.Database;

namespace FoodyExpress.Views.Pages
{
    public partial class AdminDashboard : UserControl
    {
        public AdminDashboard()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    TxtTotalUsers.Text = GetCount(conn, "Users").ToString();
                    TxtTotalOrders.Text = GetCount(conn, "Orders").ToString();
                    TxtTotalRestaurants.Text = GetCount(conn, "Restaurants").ToString();
                    TxtTotalMenuItems.Text = GetCount(conn, "MenuItems").ToString();
                }
            }
            catch (Exception ex)
            {
                Helpers.NotificationHelper.ShowError($"Failed to load dashboard data: {ex.Message}");
            }
        }

        private int GetCount(SqliteConnection conn, string table)
        {
            string query = $"SELECT COUNT(1) FROM {table}";
            using (var cmd = new SqliteCommand(query, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
