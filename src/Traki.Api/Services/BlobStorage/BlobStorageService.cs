﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace Traki.Api.Services.BlobStorage
{
    // Todo: refactor to use stream. Memory leak is possible here
    public class BlobStorageService : IStorageService
    {
        private readonly BlobStorageSettings _blobStorageSettings;

        public BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettings) 
        {
            _blobStorageSettings = blobStorageSettings.Value;
        }

        public async Task AddFile(string containerName, string fileName, string contentType, Stream content)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(_blobStorageSettings.ConnectionString, containerName); 
            blobContainerClient.CreateIfNotExists();

            BlobUploadOptions options = new BlobUploadOptions();

            var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };

            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            // For performance improvement if needed https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-upload#upload-by-staging-blocks-and-then-committing-them
            await blobClient.UploadAsync(content, blobHttpHeader);
        }

        public async Task<BlobDownloadResult> GetFile(string containerName, string fileName)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(_blobStorageSettings.ConnectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
            return downloadResult;
        }
    }
}
