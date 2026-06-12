using System;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using FoodyExpress.Database;

namespace FoodyExpress.Views.Pages
{
    public partial class DashboardControl : UserControl
    {
        public DashboardControl()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        public void RefreshDashboardData()
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    using (var cmd1 = new SqliteCommand("SELECT COUNT(*) FROM Orders", conn))
                    {
                        txtTotalOrders.Text = cmd1.ExecuteScalar()?.ToString();
                    }
                    
                    using (var cmd2 = new SqliteCommand("SELECT COUNT(*) FROM MenuItems WHERE IsAvailable = 1", conn))
                    {
                        txtMenuCount.Text = cmd2.ExecuteScalar()?.ToString();
                    }
                }
            }
            catch (Exception)
            {
                // Handle or log error
            }
        }
    }
}
