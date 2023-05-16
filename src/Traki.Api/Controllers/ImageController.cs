using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Domain.Services.BlobStorage;

namespace Traki.Api.Controllers
{
    [Route("api/control")]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public ImageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPut("folders/{folderName}/files/{fileName}")]
        public async Task<ActionResult> UpdateFile(string folderName, string fileName)
        {
            var formCollection = await Request.ReadFormAsync();
            IFormFile file = formCollection.Files.First();

            await _storageService.AddFile(folderName, fileName, file.ContentType, file.OpenReadStream());

            return Ok();
        }

        [HttpPost("folders/{folderName}/files")]
        public async Task<ActionResult> UploadFile(string folderName)
        {
            var formCollection = await Request.ReadFormAsync();

            foreach (var file in formCollection.Files)
            {
                if (!IsImage(file.ContentType))
                {
                    return BadRequest();
                }
            }

            foreach (var file in formCollection.Files)
            {
                await _storageService.AddFile(folderName, file.FileName, file.ContentType, file.OpenReadStream());
            }

            return Ok();
        }

        [HttpGet("folders/{folderName}/files/{fileName}")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<ActionResult> GetFile(string folderName, string fileName)
        {
            var file = await _storageService.GetFile(folderName, fileName);

            return File(file.Content, file.ContentType);
        }

        private bool IsImage(string contentType)
        {
            return contentType.StartsWith("image");
        }
    }
}
