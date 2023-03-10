using CarRental.Domain.Entities;

namespace CarRental.Application.Interfaces.Repositories
{
    public interface ICarReservationRepository : IDataRepository<CarReservation>
    {
        Task<CarReservation> GetExistingReservationByCarIdAsync(string carId, DateTime rentFrom);
    }
}
