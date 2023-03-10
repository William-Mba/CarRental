namespace CarRental.Domain.Entities
{
    public class Car : BaseEntity
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
