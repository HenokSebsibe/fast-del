using System;

namespace FoodyExpress.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int? DeliveryPersonID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation properties (optional for raw ADO.NET but helpful)
        public string? CustomerName { get; set; }
    }
}
