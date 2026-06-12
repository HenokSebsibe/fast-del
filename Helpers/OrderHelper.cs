using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using FoodyExpress.Models;
using FoodyExpress.Database;

namespace FoodyExpress.Helpers
{
    public static class OrderHelper
    {
        public static void CreateOrder(int customerId, List<CartItem> cartItems, decimal totalAmount)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string orderQuery = @"
                            INSERT INTO Orders (CustomerID, OrderDate, Status, TotalAmount) 
                            VALUES (@CustomerId, @OrderDate, 'Pending', @TotalAmount);
                            SELECT last_insert_rowid();";
                        
                        int orderId = 0;
                        using (var cmd = new SqliteCommand(orderQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CustomerId", customerId);
                            cmd.Parameters.AddWithValue("@OrderDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                            orderId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string itemQuery = @"
                            INSERT INTO OrderItems (OrderID, MenuItemID, Quantity, UnitPrice) 
                            VALUES (@OrderId, @MenuItemId, @Quantity, @UnitPrice)";
                        
                        foreach (var item in cartItems)
                        {
                            using (var cmd = new SqliteCommand(itemQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@OrderId", orderId);
                                cmd.Parameters.AddWithValue("@MenuItemId", item.MenuItem!.MenuItemID);
                                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                cmd.Parameters.AddWithValue("@UnitPrice", item.MenuItem!.Price);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
