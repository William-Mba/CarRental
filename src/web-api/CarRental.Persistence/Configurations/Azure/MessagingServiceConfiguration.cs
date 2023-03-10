using CarRental.Application.Interfaces.Configuration;
using Microsoft.Extensions.Options;

namespace CarRental.Persistence.Configurations.Azure
{
    public class MessagingServiceConfiguration : IMessagingServiceConfiguration
    {
        public string ListenAndSendConnectionString { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
    }

    public class MessagingServiceConfigurationValidation : IValidateOptions<MessagingServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string? name, MessagingServiceConfiguration options)
        {
            if (string.IsNullOrWhiteSpace(options.ListenAndSendConnectionString))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ListenAndSendConnectionString)} configuration parameter for the Azure Service Bus is required.");
            }

            if (string.IsNullOrWhiteSpace(options.QueueName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.QueueName)} configuration parameter for the Azure Service Bus is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
