using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.Text;
using Traki.Domain.Constants;
using Traki.Domain.Handlers;
using Traki.Domain.Services.Docusign;

namespace Traki.Api.Controllers
{
    [Route("api/protocols/{protocolId}/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportHandler _reportsHandler;
        private readonly IDocuSignService _docuSignService;
        private readonly IMemoryCache _memoryCache;

        public ReportsController(IDocuSignService docuSignService, IMemoryCache memoryCache, IReportHandler reportsHandler)
        {
            _docuSignService = docuSignService;
            _reportsHandler = reportsHandler;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GenerateReport()
        {
            var a = await _reportsHandler.GenerateHtmlReport();

            var b =  Convert.ToBase64String(a);
            return b;
        }

        [HttpPost("sign")]
        [Authorize]
        public async Task<ActionResult<string>> SignDocument()
        {
            var reportFile = await _reportsHandler.GenerateHtmlReport();

            string report = Convert.ToBase64String(reportFile);

            bool exists = _memoryCache.TryGetValue<string>(GetUserId(), out string accessToken);

            if (!exists)
            {
                return BadRequest();
            }

            int signerClientId = 1000;

            var userInfo = await _docuSignService.GetUserInformation(accessToken);

            // refactor....
            var result = _docuSignService.SendEnvelopeForEmbeddedSigning(userInfo.Email, userInfo.Name, signerClientId.ToString(),
                accessToken, userInfo.Accounts.First().BaseUri + "/restapi", userInfo.Accounts.First().AccountId, report, "http://localhost:3000");

            return Ok(result.Item2);
        }

        private int GetUserId()
        {
            return int.Parse(User.Claims.FirstOrDefault(x => x.Type == Claims.UserId).Value);
        }
    }
}
