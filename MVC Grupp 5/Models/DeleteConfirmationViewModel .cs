namespace MVC_Grupp_5.Models
{
    public class DeleteConfirmationViewModel
    {
        public string VehicleId { get; set; }
        public bool ReceiptRequested { get; set; }

        public string? RegNr { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public string? VehicleType { get; set; }
        public DateTime CheckInVehicle { get; set; }

        public TimeSpan CheckInTimeDifference { get; set; }
        public DeleteConfirmationViewModel()
        {
            VehicleId = string.Empty;
        }
    }

}
