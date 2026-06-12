using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using FoodyExpress.Models;
using FoodyExpress.Database;

namespace FoodyExpress.Views.Pages
{
    public partial class OrderManagement : UserControl
    {
        public OrderManagement()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                var orders = new List<Order>();
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string query = "SELECT * FROM Orders ORDER BY OrderDate DESC";
                    using (var cmd = new SqliteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                                DeliveryPersonID = reader["DeliveryPersonID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["DeliveryPersonID"]),
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                Status = reader["Status"].ToString(),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"])
                            });
                        }
                    }
                }
                DataGridOrders.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                Helpers.NotificationHelper.ShowError($"Error loading orders: {ex.Message}");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtOrderId.Text.Trim(), out int orderId))
            {
                Helpers.NotificationHelper.ShowError("Invalid Order ID.");
                return;
            }

            if (CmbStatus.SelectedItem == null)
            {
                Helpers.NotificationHelper.ShowError("Please select a status.");
                return;
            }

            string? status = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

            try
            {
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string query = "UPDATE Orders SET Status = @Status WHERE OrderID = @OrderId";
                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@OrderId", orderId);
                        cmd.ExecuteNonQuery();
                    }
                }

                Helpers.NotificationHelper.ShowInfo("Order updated successfully!");
                TxtOrderId.Clear();
                CmbStatus.SelectedIndex = -1;
                LoadOrders();
            }
            catch (Exception ex)
            {
                Helpers.NotificationHelper.ShowError($"Error updating order: {ex.Message}");
            }
        }
    }
}
