namespace FoodyExpress.Models
{
    public class DeliveryPersonnel
    {
        public int DeliveryPersonID { get; set; }
        public int UserID { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; }
    }
}
