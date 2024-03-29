﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Traki.Domain.Services.BlobStorage;
using Traki.Domain.Services.FileStorage;

namespace Traki.Infrastructure.Services
{
    public class BlobStorageService : IStorageService
    {
        private readonly StorageSettings _blobStorageSettings;

        public BlobStorageService(IOptions<StorageSettings> blobStorageSettings)
        {
            _blobStorageSettings = blobStorageSettings.Value;
        }

        public async Task AddFile(string containerName, string fileName, string contentType, Stream content)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(_blobStorageSettings.ConnectionString, containerName);
            blobContainerClient.CreateIfNotExists();

            var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };

            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            // For performance improvement if needed https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-upload#upload-by-staging-blocks-and-then-committing-them
            var response = await blobClient.UploadAsync(content, blobHttpHeader);
            return;
        }

        public async Task<GetFileResult> GetFile(string containerName, string fileName)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(_blobStorageSettings.ConnectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
            return new GetFileResult { 
                Content = downloadResult.Content.ToArray(),
                ContentType = downloadResult.Details.ContentType
            };
        }
    }
}
