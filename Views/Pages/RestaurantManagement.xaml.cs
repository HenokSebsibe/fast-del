using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using FoodyExpress.Models;
using FoodyExpress.Database;

namespace FoodyExpress.Views.Pages
{
    public partial class RestaurantManagement : UserControl
    {
        public RestaurantManagement()
        {
            InitializeComponent();
            LoadRestaurants();
        }

        private void LoadRestaurants()
        {
            try
            {
                var restaurants = new List<Restaurant>();
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string query = "SELECT * FROM Restaurants";
                    using (var cmd = new SqliteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            restaurants.Add(new Restaurant
                            {
                                RestaurantID = Convert.ToInt32(reader["RestaurantID"]),
                                RestaurantName = reader["RestaurantName"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Address = reader["Address"].ToString(),
                                IsActive = Convert.ToInt32(reader["IsActive"]) == 1
                            });
                        }
                    }
                }
                DataGridRestaurants.ItemsSource = restaurants;
            }
            catch (Exception ex)
            {
                Helpers.NotificationHelper.ShowError($"Error loading restaurants: {ex.Message}");
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text.Trim();
            string phone = TxtPhone.Text.Trim();
            string address = TxtAddress.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                Helpers.NotificationHelper.ShowError("Restaurant Name is required.");
                return;
            }

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string query = "INSERT INTO Restaurants (RestaurantName, Phone, Address, IsActive) VALUES (@Name, @Phone, @Address, 1)";
                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.ExecuteNonQuery();
                    }
                }

                Helpers.NotificationHelper.ShowInfo("Restaurant added successfully!");
                TxtName.Clear();
                TxtPhone.Clear();
                TxtAddress.Clear();
                LoadRestaurants();
            }
            catch (Exception ex)
            {
                Helpers.NotificationHelper.ShowError($"Error adding restaurant: {ex.Message}");
            }
        }
    }
}
