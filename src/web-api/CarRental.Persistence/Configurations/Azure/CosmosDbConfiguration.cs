using CarRental.Application.Interfaces.Configuration;
using Microsoft.Extensions.Options;

namespace CarRental.Persistence.Configurations.Azure
{
    public class CosmosDbConfiguration : ICosmosDbConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string CarContainerName { get; set; } = string.Empty;
        public string CarContainerPartitionKeyPath { get; set; } = string.Empty;
        public string EnquiryContainerName { get; set; } = string.Empty;
        public string EnquiryContainerPartitionKeyPath { get; set; } = string.Empty;
        public string CarReservationContainerName { get; set; } = string.Empty;
        public string CarReservationContainerPartitionKeyPath { get; set; } = string.Empty;
    }

    public class CosmosDbConfigurationValidation : IValidateOptions<CosmosDbConfiguration>
    {
        public ValidateOptionsResult Validate(string? name, CosmosDbConfiguration options)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ConnectionString)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrWhiteSpace(options.DatabaseName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.DatabaseName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrWhiteSpace(options.CarContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CarContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrWhiteSpace(options.CarContainerPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CarContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrWhiteSpace(options.EnquiryContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.EnquiryContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrWhiteSpace(options.EnquiryContainerPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.EnquiryContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrWhiteSpace(options.CarReservationContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CarReservationContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrWhiteSpace(options.CarReservationContainerPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CarReservationContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
