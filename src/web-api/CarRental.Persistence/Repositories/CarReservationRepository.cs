using CarRental.Application.Interfaces.Configuration;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Serilog;
using System.Net;

namespace CarRental.Persistence.Repositories
{
    public class CarReservationRepository : CosmosDbDataRepository<CarReservation>, ICarReservationRepository
    {
        public override string ContainerName => _cosmosDbConfiguration.CarReservationContainerName;

        public CarReservationRepository(ICosmosDbConfiguration cosmosDbConfiguration, CosmosClient cosmosClient)
            : base(cosmosDbConfiguration, cosmosClient)
        {
        }

        public async Task<CarReservation> GetExistingReservationByCarIdAsync(string carId, DateTime rentFrom)
        {
            try
            {
                Container container = GetContainer();

                var entities = new List<CarReservation>();

                QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.rentTo > @rentFrom AND c.carId = @carId")
                    .WithParameter("@rentFrom", rentFrom)
                    .WithParameter("@carId", carId);

                FeedIterator<CarReservation> feed = container.GetItemQueryIterator<CarReservation>(queryDefinition);

                while (feed.HasMoreResults)
                {
                    FeedResponse<CarReservation> response = await feed.ReadNextAsync();

                    foreach (var item in response)
                    {
                        entities.Add(item);
                    }
                }
                return entities.FirstOrDefault() ?? throw new ArgumentNullException(nameof(entities));
            }
            catch (CosmosException ex)
            {
                Log.Error($"Entity with ID: {carId} was not retrieve successfully - error details: {ex.Message}");

                if (ex.StatusCode != HttpStatusCode.NotFound)
                {
                    throw;
                }
                return null!;
            }
        }
    }
}
