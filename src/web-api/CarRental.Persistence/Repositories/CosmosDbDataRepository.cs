using CarRental.Application.Interfaces.Configuration;
using CarRental.Application.Interfaces.Repositories;
using CarRental.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Serilog;
using System.Net;

namespace CarRental.Persistence.Repositories
{
    public abstract class CosmosDbDataRepository<T> : IDataRepository<T> where T : BaseEntity
    {
        protected readonly ICosmosDbConfiguration _cosmosDbConfiguration;

        protected readonly CosmosClient _cosmosClient;

        public abstract string ContainerName { get; }

        public CosmosDbDataRepository(ICosmosDbConfiguration cosmosDbConfiguration, CosmosClient cosmosClient)
        {
            _cosmosDbConfiguration = cosmosDbConfiguration ?? throw new
                ArgumentNullException(nameof(cosmosDbConfiguration), $"{nameof(cosmosDbConfiguration)} cannot be null.");

            _cosmosClient = cosmosClient ?? throw new
                ArgumentNullException(nameof(cosmosClient), $"{nameof(cosmosClient)} cannot be null.");
        }

        public async Task<T> AddAsync(T newEntity)
        {
            try
            {
                Container container = GetContainer();

                ItemResponse<T> response = await container.CreateItemAsync(newEntity);

                return response.Resource;
            }
            catch (CosmosException ex)
            {
                Log.Error($"New entity with ID: {newEntity.Id} was not added successfully - error details: {ex.Message}");

                if (ex.StatusCode != HttpStatusCode.NotFound)
                {
                    throw;
                }
                return null!;
            }
        }

        public async Task DeleteAsync(string entityId)
        {
            try
            {
                Container container = GetContainer();

                await container.DeleteItemAsync<T>(entityId, new PartitionKey(entityId));
            }
            catch (CosmosException ex)
            {

                Log.Error($"Entity with ID: {entityId} was not removed successfully - error details: {ex.Message}");

                if (ex.StatusCode != HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            try
            {
                Container container = GetContainer();

                FeedIterator<T> feed = container.GetItemQueryIterator<T>();

                List<T> entities = new List<T>();

                while (feed.HasMoreResults)
                {
                    FeedResponse<T> reponse = await feed.ReadNextAsync();

                    foreach (var entity in reponse)
                    {
                        entities.Add(entity);
                    }
                }

                return entities;
            }
            catch (CosmosException ex)
            {

                Log.Error($"Entities was not retrieved successfully - error details: {ex.Message}");

                if (ex.StatusCode != HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null!;
            }
        }

        public async Task<T> GetAsync(string entityId)
        {
            try
            {
                Container container = GetContainer();

                ItemResponse<T> response = await container.ReadItemAsync<T>(entityId, new PartitionKey(entityId));

                return response.Resource;
            }
            catch (CosmosException ex)
            {

                Log.Error($"Entity with ID: {entityId} was not retrieved successfully - error details: {ex.Message}");

                if (ex.StatusCode != HttpStatusCode.NotFound)
                {
                    throw;
                }
                return null!;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                Container container = GetContainer();

                string entityId = entity.Id.ToString();

                ItemResponse<T> response = await container.ReadItemAsync<T>(entityId, new PartitionKey(entityId));

                if (response is not null)
                {
                    await container.ReplaceItemAsync(entity, entityId, new PartitionKey(entityId));
                }

                return entity;
            }
            catch (CosmosException ex)
            {
                Log.Error($"Entity with ID: {entity.Id} was not updated successfully - error details: {ex.Message}");

                if (ex.StatusCode != HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null!;
            }
        }

        protected Container GetContainer()
        {
            var database = _cosmosClient.GetDatabase(_cosmosDbConfiguration.DatabaseName);

            var container = database.GetContainer(ContainerName);

            return container;
        }
    }
}
