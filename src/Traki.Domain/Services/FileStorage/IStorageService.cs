using Azure.Storage.Blobs.Models;
using Traki.Domain.Services.FileStorage;

namespace Traki.Domain.Services.BlobStorage
{
    public interface IStorageService
    {
        Task AddFile(string containerName, string fileName, string contentType, Stream content);
        Task<GetFileResult> GetFile(string containerName, string fileName);
    }
}
