namespace CarRental.Domain.Entities
{
    public class CarReservation : BaseEntity
    {
        public string CustomerId { get; set; } = string.Empty;
        public string CarId { get; set; } = string.Empty;
        public DateTime RentFrom { get; set; }
        public DateTime RentTo { get; set; }
    }
}
