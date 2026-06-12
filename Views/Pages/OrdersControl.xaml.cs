using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using FoodyExpress.Database;
using FoodyExpress.Models;

namespace FoodyExpress.Views.Pages
{
    public partial class OrdersControl : UserControl
    {
        private string _role;
        private int _userId;

        public OrdersControl(string role, int userId)
        {
            InitializeComponent();
            _role = role;
            _userId = userId;
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                var ordersList = new List<Order>();
                using (var conn = DatabaseHelper.GetConnection())
                {
                    string query = @"
                        SELECT o.OrderID, o.OrderDate, o.Status, o.TotalAmount, c.CustomerName 
                        FROM Orders o
                        JOIN Customers c ON o.CustomerID = c.CustomerID";

                    // Basic role-based filtering
                    if (_role == "Customer")
                    {
                        query += " WHERE c.UserID = @UserId";
                    }
                    else if (_role == "Delivery")
                    {
                        query += " JOIN DeliveryPersonnel dp ON o.DeliveryPersonID = dp.DeliveryPersonID WHERE dp.UserID = @UserId";
                    }

                    query += " ORDER BY o.OrderDate DESC";

                    using (var cmd = new SqliteCommand(query, conn))
                    {
                        if (_role == "Customer" || _role == "Delivery")
                        {
                            cmd.Parameters.AddWithValue("@UserId", _userId);
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ordersList.Add(new Order
                                {
                                    OrderID = reader.GetInt32(0),
                                    OrderDate = reader.GetDateTime(1),
                                    Status = reader.GetString(2),
                                    TotalAmount = reader.GetDecimal(3),
                                    CustomerName = reader.GetString(4)
                                });
                            }
                        }
                    }
                }
                dgOrders.ItemsSource = ordersList;
            }
            catch (Exception ex)
            {
                // Handle error
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
