using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using FoodyExpress.Database;
using FoodyExpress.Models;
using ModelMenuItem = FoodyExpress.Models.MenuItem;

namespace FoodyExpress.Views.Pages
{
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            InitializeComponent();
            LoadMenu();
        }

        private void LoadMenu()
        {
            try
            {
                var menuList = new List<ModelMenuItem>();
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string query = "SELECT MenuItemID, RestaurantID, ItemName, Description, Price, IsAvailable FROM MenuItems";
                    using (var cmd = new SqliteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            menuList.Add(new ModelMenuItem
                            {
                                MenuItemID = reader.GetInt32(0),
                                RestaurantID = reader.GetInt32(1),
                                ItemName = reader.GetString(2),
                                Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                IsAvailable = reader.GetInt32(5) == 1
                            });
                        }
                    }
                }
                dgMenu.ItemsSource = menuList;
            }
            catch (Exception)
            {
                // Handle error
            }
        }
    }
}
