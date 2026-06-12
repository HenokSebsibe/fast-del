namespace FoodyExpress.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public int DeliveryPersonID { get; set; }
        public string? PlateNumber { get; set; }
        public string? Model { get; set; }
        public bool IsActive { get; set; }
    }
}
