namespace FoodyExpress.Models
{
    public class Restaurant
    {
        public int RestaurantID { get; set; }
        public string? RestaurantName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
    }
}
