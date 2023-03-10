using Azure.Messaging.ServiceBus;
using CarRental.Application.Interfaces.Message;
using CarRental.Domain.Entities;
using CarRental.Persistence.Services.Models;
using System.Text.Json;

namespace CarRental.Persistence.Services.Messages
{
    public class CarReservationMessagingService : ICarReservationMessagingService
    {
        private readonly ServiceBusSender _serviceBusSender;

        public CarReservationMessagingService(ServiceBusSender serviceBusSender)
        {
            _serviceBusSender = serviceBusSender ?? throw new
                ArgumentNullException(nameof(serviceBusSender), $"{serviceBusSender} cannot be null.");
        }

        public async Task PublishNewCarReservationMessageAsync(CarReservation carReservation)
        {
            var carReservationIntegrationMessage = new CarReservationIntegrationMessage
            {
                Id = carReservation.Id,
                CarId = carReservation.CarId,
                CustomerId = carReservation.CustomerId,
                RentFrom = carReservation.RentFrom,
                RentTo = carReservation.RentTo
            };

            var serializedMessage = JsonSerializer.Serialize(carReservationIntegrationMessage);

            ServiceBusMessage message = new ServiceBusMessage(serializedMessage);

            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}
