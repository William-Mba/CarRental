using CarRental.Application.Interfaces.Configuration;
using Microsoft.Extensions.Options;

namespace CarRental.Persistence.Configurations.Azure
{
    public class BlobStorageServiceConfiguration : IBlobStorageServiceConfiguration
    {
        public string ContainerName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
    }

    public class BlobStorageServiceConfigurationValidation : IValidateOptions<BlobStorageServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string? name, BlobStorageServiceConfiguration options)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ConnectionString)} configuration parameter for the Azure Storage Account is required");
            }

            if (string.IsNullOrWhiteSpace(options.ContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ContainerName)} configuration parameter for the Azure Storage Account is required");
            }

            if (string.IsNullOrWhiteSpace(options.Key))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Key)} configuration parameter for the Azure Storage Account is required");
            }

            if (string.IsNullOrWhiteSpace(options.AccountName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.AccountName)} configuration parameter for the Azure Storage Account is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
