using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace MVC_Grupp_5.Models
{
    public class Vehicle
    {
        [Key]
        [StringLength(6)]
        [Display(Name = "Registration Number")]
        public string RegNr { get; set; } = string.Empty;
        public string? Model { get; set; }
        public string? Color { get; set; }
        [Required]
        [Display(Name = "Vehicle Type")]
        public VehicleType VehicleType { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Check In Parking Date")]
        public DateTime CheckInVehicle { get; set; }


    }
}
