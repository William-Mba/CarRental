using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using CarRental.Application.Interfaces.Configuration;
using CarRental.Application.Interfaces.Storage;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;

namespace CarRental.Persistence.Services.Storage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobStorageServiceConfiguration _blobStorageServiceconfiguration;

        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(IBlobStorageServiceConfiguration blobStorageServiceConfiguration,
            BlobServiceClient blobServiceClient)
        {
            _blobStorageServiceconfiguration = blobStorageServiceConfiguration ??
                throw new ArgumentNullException(nameof(blobStorageServiceConfiguration),
                $"{blobStorageServiceConfiguration} cannot be null.");

            _blobServiceClient = blobServiceClient ??
                throw new ArgumentNullException(nameof(blobServiceClient), $"{blobServiceClient} cannot be null.");
        }
        public async Task DeleteBlobIfExistsAsync(string blobName)
        {
            try
            {
                var container = await GetBlobContainerAsync();

                var blockBlob = container.GetBlobClient(blobName);

                await blockBlob.DeleteIfExistsAsync();
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Document {blobName} was not deleted successfully - error details: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DoesBlobExistsAsync(string blobName)
        {
            try
            {
                var container = await GetBlobContainerAsync();

                var blockBlob = container.GetBlobClient(blobName);

                var doesBlobExist = await blockBlob.ExistsAsync();

                return doesBlobExist.Value;
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Document {blobName} existence cannot be verified - error details: {ex.Message}");
                throw;
            }
        }

        public async Task DownloadBlobIfExistsAsync(Stream stream, string blobName)
        {
            try
            {
                var container = await GetBlobContainerAsync();

                var blockBlob = container.GetBlobClient(blobName);

                await blockBlob.DownloadToAsync(stream);
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Cannot download document {blobName} - error details: {ex.Message}");

                if (ex.ErrorCode != StatusCodes.Status404NotFound.ToString())
                {
                    throw;
                }
            }
        }

        public async Task<string> GetBlobUrlc(string blobName)
        {
            try
            {
                var container = await GetBlobContainerAsync();

                var blob = container.GetBlobClient(blobName);

                string blobUrl = blob.Uri.AbsoluteUri;

                return blobUrl;
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Url for document {blobName} was not found - error details: {ex.Message}");
                throw;
            }
        }

        public async Task<string> UploadBlobAsync(Stream stream, string blobName)
        {
            try
            {
                Debug.Assert(stream.CanSeek);

                stream.Seek(0, SeekOrigin.Begin);

                var container = await GetBlobContainerAsync();

                BlobClient blob = container.GetBlobClient(blobName);

                await blob.UploadAsync(stream);

                return blob.Uri.AbsoluteUri;
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Document {blobName} was not uploaded successfully - error details: {ex.Message}");
                throw;
            }
        }
        public string GenerateSasTokenForContainer()
        {
            BlobSasBuilder builder = new BlobSasBuilder();

            builder.BlobContainerName = _blobStorageServiceconfiguration.ContainerName;
            builder.ContentType = "video/mp4";
            builder.SetPermissions(BlobAccountSasPermissions.Read);
            builder.StartsOn = DateTimeOffset.UtcNow;
            builder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(90);

            var sasToken = builder.ToSasQueryParameters(
                new StorageSharedKeyCredential(
                    _blobStorageServiceconfiguration.AccountName,
                    _blobStorageServiceconfiguration.Key
                    )
                ).ToString();

            return sasToken;
        }

        private async Task<BlobContainerClient> GetBlobContainerAsync()
        {
            try
            {
                BlobContainerClient blobContainer = _blobServiceClient
                    .GetBlobContainerClient(_blobStorageServiceconfiguration.ContainerName);

                await blobContainer.CreateIfNotExistsAsync();

                return blobContainer;
            }
            catch (RequestFailedException ex)
            {
                Log.Error($"Cannot find blob container: {_blobStorageServiceconfiguration.ContainerName} - error details: {ex.Message}");
                throw;
            }
        }
    }
}
