namespace CarRental.Persistence.Services.Models
{
    public class CarReservationIntegrationMessage : IntegrationMessage
    {
        public string CustomerId { get; set; } = string.Empty;

        public string CarId { get; set; } = string.Empty;

        public DateTime RentFrom { get; set; }

        public DateTime RentTo { get; set; }
    }
}
