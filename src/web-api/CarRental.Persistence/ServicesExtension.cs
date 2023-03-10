using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using CarRental.Application.Interfaces.Configuration;
using CarRental.Application.Interfaces.Identity;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Application.Interfaces.Services;
using CarRental.Application.Interfaces.Storage;
using CarRental.Persistence.Configurations.Azure;
using CarRental.Persistence.Repositories;
using CarRental.Persistence.Services.CarReservations;
using CarRental.Persistence.Services.Identity;
using CarRental.Persistence.Services.Storage;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace CarRental.Persistence
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddPersistenceExtensions(this IServiceCollection services, IConfiguration config)
        {
            services.AddDataServices(config);

            services.AddServiceBus(config);

            return services;
        }
        private static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCosmosDBServices(config);

            services.AddApplicationServices();

            services.AddRepositories();

            services.AddBlobStorageServices(config);

            return services;
        }

        private static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MessagingServiceConfiguration>(config.GetSection("ServiceBusSettings"));

            services.AddSingleton<IValidateOptions<MessagingServiceConfiguration>, MessagingServiceConfigurationValidation>();

            services.AddSingleton<IMessagingServiceConfiguration, MessagingServiceConfiguration>();

            services.TryAddSingleton(implementationFactory =>
            {
                var serviceBusConfiguration = implementationFactory.GetRequiredService<IMessagingServiceConfiguration>();

                var serviveBusClient = new ServiceBusClient(serviceBusConfiguration.ListenAndSendConnectionString);

                var serviceBusSender = serviveBusClient.CreateSender(serviceBusConfiguration.QueueName);

                return serviceBusSender;
            });
            return services;
        }
        private static IServiceCollection AddCosmosDBServices(this IServiceCollection services, IConfiguration config)
        {

            services.Configure<CosmosDbConfiguration>(config.GetSection("CosmosDbSettings"));

            services.AddSingleton<IValidateOptions<CosmosDbConfiguration>, CosmosDbConfigurationValidation>();

            services.AddSingleton<ICosmosDbConfiguration, CosmosDbConfiguration>();

            services.TryAddSingleton(implementationFactory =>
            {
                var cosmosDbConfiguration = implementationFactory.GetRequiredService<ICosmosDbConfiguration>();

                CosmosClient cosmosClient = new CosmosClient(cosmosDbConfiguration.ConnectionString);

                Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDbConfiguration.DatabaseName)
                    .GetAwaiter()
                    .GetResult();

                database.CreateContainerIfNotExistsAsync(
                    cosmosDbConfiguration.CarContainerName,
                    cosmosDbConfiguration.CarContainerPartitionKeyPath,
                    400)
                    .GetAwaiter()
                    .GetResult();

                database.CreateContainerIfNotExistsAsync(
                    cosmosDbConfiguration.EnquiryContainerName,
                    cosmosDbConfiguration.EnquiryContainerPartitionKeyPath,
                    400)
                    .GetAwaiter()
                    .GetResult();

                database.CreateContainerIfNotExistsAsync(
                    cosmosDbConfiguration.CarReservationContainerName,
                    cosmosDbConfiguration.CarReservationContainerPartitionKeyPath,
                    400)
                    .GetAwaiter()
                    .GetResult();

                return cosmosClient;
            });

            return services;
        }
        private static IServiceCollection AddBlobStorageServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<BlobStorageServiceConfiguration>(config.GetSection("BlobStorageSettings"));

            services.AddSingleton<IValidateOptions<BlobStorageServiceConfiguration>, BlobStorageServiceConfigurationValidation>();

            services.AddSingleton<IBlobStorageServiceConfiguration, BlobStorageServiceConfiguration>();

            var serviceProvider = services.BuildServiceProvider();

            var storageConfiguration = serviceProvider.GetRequiredService<IBlobStorageServiceConfiguration>();

            services.AddSingleton(factory => new BlobServiceClient(storageConfiguration.ConnectionString));

            services.AddSingleton<IBlobStorageService, BlobStorageService>();

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<ICarReservationService, CarReservationService>();

            services.AddTransient<IIdentityService, IdentityService>();

            return services;

        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ICarRepository, CarRepository>();

            services.AddSingleton<IEnquiryRepository, EnquiryRepository>();

            services.AddSingleton<ICarReservationRepository, CarReservationRepository>();

            return services;

        }

    }
}
