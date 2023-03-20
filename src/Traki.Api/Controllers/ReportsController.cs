using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Traki.Api.Constants;
using Traki.Api.Handlers;
using Traki.Api.Services;
using Traki.Api.Services.Docusign;

namespace Traki.Api.Controllers
{
    [Route("api/reports")]
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
        [Authorize]
        public async Task<ActionResult<string>> GenerateReport()
        {
            return _reportsHandler.GenerateReport();
        }

        [HttpPost("sign")]
        [Authorize]
        public async Task<ActionResult<string>> SignDocument()
        {
            string report = _reportsHandler.GenerateReport();

            bool exists = _memoryCache.TryGetValue<string>(GetUserId(), out string accessToken);

            if (!exists)
            {
                return BadRequest();
            }

            int signerClientId = 1000;

            var userInfo = await _docuSignService.GetUserInformation(accessToken);

            // refactor....
            var result = _docuSignService.SendEnvelopeForEmbeddedSigning(userInfo.Email, $"{userInfo.Name} {userInfo.FamilName}", signerClientId.ToString(),
                accessToken, userInfo.Accounts.First().BaseUri + "/restapi", userInfo.Accounts.First().AccountId, report, "http://localhost:3000");

            return Ok(result.Item2);
        }

        private int GetUserId()
        {
            return int.Parse(User.Claims.FirstOrDefault(x => x.Type == Claims.UserId).Value);
        }
    }
}
