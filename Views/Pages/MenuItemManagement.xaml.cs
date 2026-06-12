using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using FoodyExpress.Models;
using FoodyExpress.Database;

namespace FoodyExpress.Views.Pages
{
    public partial class MenuItemManagement : UserControl
    {
        public MenuItemManagement()
        {
            InitializeComponent();
            LoadMenuItems();
        }

        private void LoadMenuItems()
        {
            try
            {
                var items = new List<FoodyExpress.Models.MenuItem>();
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string query = "SELECT * FROM MenuItems";
                    using (var cmd = new SqliteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new FoodyExpress.Models.MenuItem
                            {
                                MenuItemID = Convert.ToInt32(reader["MenuItemID"]),
                                RestaurantID = Convert.ToInt32(reader["RestaurantID"]),
                                ItemName = reader["ItemName"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                IsAvailable = Convert.ToInt32(reader["IsAvailable"]) == 1
                            });
                        }
                    }
                }
                DataGridMenuItems.ItemsSource = items;
            }
            catch (Exception ex)
            {
                Helpers.NotificationHelper.ShowError($"Error loading menu items: {ex.Message}");
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtRestaurantId.Text.Trim(), out int restaurantId))
            {
                Helpers.NotificationHelper.ShowError("Invalid Restaurant ID.");
                return;
            }

            string name = TxtName.Text.Trim();
            if (!decimal.TryParse(TxtPrice.Text.Trim(), out decimal price))
            {
                Helpers.NotificationHelper.ShowError("Invalid Price.");
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                Helpers.NotificationHelper.ShowError("Menu Item Name is required.");
                return;
            }

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string query = "INSERT INTO MenuItems (RestaurantID, ItemName, Price, IsAvailable) VALUES (@RestId, @Name, @Price, 1)";
                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RestId", restaurantId);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.ExecuteNonQuery();
                    }
                }

                Helpers.NotificationHelper.ShowInfo("Menu Item added successfully!");
                TxtName.Clear();
                TxtPrice.Clear();
                LoadMenuItems();
            }
            catch (Exception ex)
            {
                Helpers.NotificationHelper.ShowError($"Error adding menu item: {ex.Message}");
            }
        }
    }
}
