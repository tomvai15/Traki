using Azure.Storage.Blobs.Models;

namespace Traki.Api.Services.BlobStorage
{
    public interface IStorageService
    {
        Task AddFile(string containerName, string fileName, string contentType, Stream content);
        Task<BlobDownloadResult> GetFile(string containerName, string fileName);
    }
}
