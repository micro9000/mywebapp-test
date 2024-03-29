﻿using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace MyWebAppTest.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private string _accountName;
        private string _containerName;

        public BlobStorageService(IConfiguration configuration)
        {
            _accountName = configuration["blobStorageAccountName"].ToString();
            _containerName = configuration["blobContainerName"].ToString();
        }

        public BlobContainerClient GetBlobContainerClient(string accountName, string containerName)
        {
            string containerEndpoint = string.Format("https://{0}.blob.core.windows.net/{1}",
                                                        accountName,
                                                        containerName);

            BlobContainerClient containerClient = new BlobContainerClient(new Uri(containerEndpoint),
                                                                            new DefaultAzureCredential());

            return containerClient;
        }

        public async Task UploadBlob(Stream stream, string fileName)
        {
            var containerClient = this.GetBlobContainerClient(_accountName, _containerName);

            await containerClient.CreateIfNotExistsAsync();

            await containerClient.UploadBlobAsync(fileName, stream);

        }
    }
}
