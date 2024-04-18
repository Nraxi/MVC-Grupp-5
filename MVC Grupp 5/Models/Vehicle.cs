using System.ComponentModel.DataAnnotations;
namespace MVC_Grupp_5.Models
{
    public class Vehicle
    {
        [Key]
        [StringLength(6)]
        public string RegNr { get; set; } = string.Empty;
        public string? Model { get; set; }
        public string? Color { get; set; }
        [Required]
        public VehicleType VehicleType { get; set; }
        public DateTime CheckInVehicle { get; set; }


    }
}
