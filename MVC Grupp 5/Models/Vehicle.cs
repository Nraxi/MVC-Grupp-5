using System.ComponentModel.DataAnnotations;
namespace MVC_Grupp_5.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        [StringLength(6)]
        public string RegNr { get; set; } = string.Empty;
        public string? Model { get; set; }
        public string? Color { get; set; }
        public VehicleType VehicleType { get; set; }

    }
}
