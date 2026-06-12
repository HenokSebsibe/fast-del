using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace FoodyExpress.Database
{
    public static class DatabaseHelper
    {
        // For a desktop app, storing it in the AppData or local directory is best.
        public static string DbFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "foodyexpress.db");
        public static string ConnectionString => $"Data Source={DbFilePath}";

        public static void InitializeDatabase()
        {
            bool isNew = !File.Exists(DbFilePath);
            
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                
                // Read and execute schema.sql
                string schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "schema.sql");
                if (File.Exists(schemaPath))
                {
                    string schema = File.ReadAllText(schemaPath);
                    using (var command = new SqliteCommand(schema, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // Helper method to get a connection easily
        public static SqliteConnection GetConnection()
        {
            var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
