namespace CarRental.Application.Interfaces.Storage
{
    public interface IBlobStorageService
    {
        Task DownloadBlobIfExistsAsync(Stream stream, string blobName);
        Task<string> UploadBlobAsync(Stream stream, string blobName);
        Task DeleteBlobIfExistsAsync(string blobName);
        Task<bool> DoesBlobExistsAsync(string blobName);
        Task<string> GetBlobUrlc(string blobName);
        string GenerateSasTokenForContainer();
    }
}
