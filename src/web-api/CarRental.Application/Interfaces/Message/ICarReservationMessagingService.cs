using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces.Message
{
    public interface ICarReservationMessagingService
    {
        Task PublishNewCarReservationMessageAsync(CarReservation carReservation);
    }
}
