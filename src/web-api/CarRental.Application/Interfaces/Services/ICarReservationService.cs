using CarRental.Application.Common;
using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces.Services
{
    public interface ICarReservationService
    {
        Task<OperationResponse<CarReservation>> MakeReservationAsync(CarReservation carReservation);
    }
}
