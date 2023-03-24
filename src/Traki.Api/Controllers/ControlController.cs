using Microsoft.AspNetCore.Mvc;
using Traki.Domain.Services.BlobStorage;
using Traki.Infrastructure.Data;

namespace Traki.Api.Controllers
{
    [Route("api/control")]
    public class ControlController : ControllerBase
    {
        private readonly TrakiDbContext _trakiDbContext;
        private readonly IStorageService _storageService;

        public ControlController(IStorageService storageService, TrakiDbContext trakiDbContext)
        {
            _storageService = storageService;
            _trakiDbContext = trakiDbContext;
        }

        [HttpGet("create")]
        public async Task<ActionResult> Initiliase()
        {
            bool wasCreated = _trakiDbContext.Database.EnsureCreated();

            if (!wasCreated) return Ok("No data was added");

            _trakiDbContext.AddProjects();
            _trakiDbContext.AddProducts();
            _trakiDbContext.AddTemplates();
            _trakiDbContext.AddQuestions();
            _trakiDbContext.AddChecklists();
            _trakiDbContext.AddChecklistQuestions();

            return Ok("Added data");
        }

        [HttpPut("folders/{folderName}/files/{fileName}")]
        public async Task<ActionResult> UpdateFile(string folderName, string fileName)
        {
            var formCollection = await Request.ReadFormAsync();
            IFormFile file = formCollection.Files.First();

            await _storageService.AddFile(folderName, fileName, file.ContentType, file.OpenReadStream());

            return Ok();
        }

        [HttpGet("folders/{folderName}/files/{fileName}")]
        public async Task<ActionResult> GetFile(string folderName, string fileName)
        {
            var file = await _storageService.GetFile(folderName, fileName);

            var bytes = file.Content.ToStream();

            return File(bytes, file.Details.ContentType);
        }
    }
}
