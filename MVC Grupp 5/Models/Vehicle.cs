using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace MVC_Grupp_5.Models
{
    public class Vehicle
    {
        [Key]
        [RegularExpression(@"^[^-\s]+$", ErrorMessage = "Registration Number cannot contain spaces or negative values.")]
        [StringLength(6, ErrorMessage = "Registration Number must be 6 characters long")]
        [Display(Name = "Registration Number")]
        [Required(ErrorMessage = "Registration Number is required.")]
        public string RegNr { get; set; } = string.Empty;
        [StringLength(20, ErrorMessage = "Model must have a limit of 20 characters long")]

        public string? Model { get; set; }
        [StringLength(20, ErrorMessage = "Color must have a limit of  20 characters long")]

        public string? Color { get; set; }
        [Required(ErrorMessage = "Vehicle Type is required.")]
        [Display(Name = "Vehicle Type")]
        public VehicleType VehicleType { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Check In / Parked Time")]
        public DateTime CheckInVehicle { get; set; }


    }
}
