using Microsoft.AspNetCore.Mvc;
using Traki.Api.Data;
using Traki.Api.Services.BlobStorage;

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

        [HttpPost("uploadfile")]
        public async Task<ActionResult> UploadFile()
        {
            var formCollection = await Request.ReadFormAsync();
            IFormFile file = formCollection.Files.First();

            /*
            MemoryStream stream = new MemoryStream();

             await file.CopyToAsync(stream);

            file.OpenReadStream

            BinaryData binaryData = new BinaryData(stream.ToArray());
            */

            string fileName = file.FileName;
            await _storageService.AddFile("product1", fileName, file.ContentType, file.OpenReadStream());

            return Ok();
        }

        [HttpGet("getfile/{fileName}")]
        public async Task<ActionResult> GetFile(string fileName)
        {
            var file = await _storageService.GetFile("product1", fileName);

            var bytes = file.Content.ToStream();

           // var bytes = file.Content.ToArray();

            return File(bytes, file.Details.ContentType);
        }
    }
}
