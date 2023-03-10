using CarRental.Application.Interfaces.Configuration;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace CarRental.Persistence.Repositories
{
    public class CarRepository : CosmosDbDataRepository<Car>, ICarRepository
    {
        public override string ContainerName => _cosmosDbConfiguration.CarContainerName;

        public CarRepository(ICosmosDbConfiguration cosmosDbConfiguration, CosmosClient cosmosClient)
            : base(cosmosDbConfiguration, cosmosClient)
        {
        }
    }
}
