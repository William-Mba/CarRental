using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos
{
    public class CarReservationDto
    {
        [Required]
        public string CarId { get; set; } = string.Empty;

        [Required]
        public DateTime RentFrom { get; set; }

        [Required]
        public DateTime RentTo { get; set; }
    }
}
