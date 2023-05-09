using Microsoft.AspNetCore.Mvc;
using Traki.Domain.Services.BlobStorage;
using Traki.Domain.Services.Email;
using Traki.Domain.Services.Notifications;
using Traki.Infrastructure.Data;

namespace Traki.Api.Controllers
{
    [Route("api/control")]
   // [Authorize]
    public class FileController : ControllerBase
    {
        private readonly TrakiDbContext _trakiDbContext;
        private readonly IStorageService _storageService;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;

        public FileController(IStorageService storageService, TrakiDbContext trakiDbContext, IEmailService emailService, INotificationService notificationService)
        {
            _storageService = storageService;
            _trakiDbContext = trakiDbContext;
            _emailService = emailService;
            _notificationService = notificationService;
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

        [HttpPost("email/{emailName}")]
        public async Task<ActionResult> SendEmail(string emailName)
        {
            await _emailService.SendEmail(emailName, "a", "b");
            return Ok();
        }

        [HttpPost("notification")]
        public async Task<ActionResult> Notification()
        {
           // await _notificationService.SendNotification();
            return Ok();
        }


        [HttpPost("folders/{folderName}/files")]
        public async Task<ActionResult> UploadFile(string folderName)
        {
            var formCollection = await Request.ReadFormAsync();

            foreach(var file in formCollection.Files)
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
    }
}
