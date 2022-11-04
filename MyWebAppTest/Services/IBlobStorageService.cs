using Azure.Storage.Blobs;

namespace MyWebAppTest.Services
{
    public interface IBlobStorageService
    {
        BlobContainerClient GetBlobContainerClient(string accountName, string containerName);
        Task UploadBlob(Stream stream, string fileName);
    }
}