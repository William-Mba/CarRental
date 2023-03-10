using CarRental.Application.Interfaces.Configuration;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace CarRental.Persistence.Repositories
{
    public class EnquiryRepository : CosmosDbDataRepository<Enquiry>, IEnquiryRepository
    {
        public override string ContainerName => _cosmosDbConfiguration.EnquiryContainerName;
        public EnquiryRepository(ICosmosDbConfiguration cosmosDbConfiguration, CosmosClient cosmosClient)
            : base(cosmosDbConfiguration, cosmosClient)
        {
        }
    }
}
