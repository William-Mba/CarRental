using CarRental.Application.Common;
using CarRental.Application.Interfaces.Identity;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;

namespace CarRental.Persistence.Services.CarReservations
{
    public class CarReservationService : ICarReservationService
    {
        private readonly ICarReservationRepository _carReservationRepository;

        private readonly ICarRepository _carRepository;

        private readonly IIdentityService _identityService;

        public CarReservationService(ICarReservationRepository carReservationRepository,
            ICarRepository carRepository, IIdentityService identityService)
        {
            _carReservationRepository = carReservationRepository ??
                throw new ArgumentNullException(nameof(carReservationRepository));

            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));

            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<OperationResponse<CarReservation>> MakeReservationAsync(CarReservation carReservation)
        {
            var carFromReservation = await _carRepository.GetAsync(carReservation.CarId);

            if (carFromReservation is null)
            {
                return new OperationResponse<CarReservation>()
                    .SetAsFailureResponse(OperationErrorDictionary.CarReservation.CarDoesNotExist());
            }

            var existingCarReservation = await _carReservationRepository
                .GetExistingReservationByCarIdAsync(carReservation.CarId, carReservation.RentFrom);

            if (existingCarReservation is not null)
            {
                return new OperationResponse<CarReservation>()
                    .SetAsFailureResponse(OperationErrorDictionary.CarReservation.CarAlreadyReserved());
            }
            else
            {
                carReservation.Id = Guid.NewGuid().ToString();
                carReservation.CustomerId = _identityService.GetUserIdentity().ToString();

                var reservation = await _carReservationRepository.AddAsync(carReservation);

                return new OperationResponse<CarReservation>(reservation);
            }
        }
    }
}
