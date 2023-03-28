using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.Text;
using Traki.Api.Contracts.Report;
using Traki.Domain.Constants;
using Traki.Domain.Handlers;
using Traki.Domain.Repositories;
using Traki.Domain.Services.Docusign;

namespace Traki.Api.Controllers
{
    [Route("api/protocols/{protocolId}/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportHandler _reportsHandler;
        private readonly IDocuSignService _docuSignService;
        private readonly IMemoryCache _memoryCache;
        private readonly IProtocolRepository _protocolRepository;

        public ReportsController(IProtocolRepository protocolRepository, IDocuSignService docuSignService, IMemoryCache memoryCache, IReportHandler reportsHandler)
        {
            _protocolRepository = protocolRepository;
            _docuSignService = docuSignService;
            _reportsHandler = reportsHandler;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GenerateReport(int protocolId)
        {
            var protocolReport = await _reportsHandler.GetProtocolReportInformation(protocolId);

            byte[] report;
            if (protocolReport.Protocol.IsSigned)
            {
                report = await _reportsHandler.GetProtocolReport(protocolId);
            }
            else
            {
                report = await _reportsHandler.GenerateHtmlReport(protocolReport);
            }

            // TODO: this can be done on client
            var reportBase64 = Convert.ToBase64String(report);
            return reportBase64;
        }

        [HttpGet("validate")]
        [Authorize]
        public async Task<ActionResult> ValidateDocumentSign(int protocolId)
        {
            bool exists = _memoryCache.TryGetValue<string>(GetUserId(), out string accessToken);

            if (!exists)
            {
                return BadRequest();
            }

            var protocol = await _protocolRepository.GetProtocol(protocolId);

            if (protocol.EnvelopeId == null)
            {
                return BadRequest();
            }

            var userInfo = await _docuSignService.GetUserInformation(accessToken);
            await _reportsHandler.ValidateSign(protocol, userInfo, accessToken);

            return Ok();
        }

        [HttpPost("sign")]
        [Authorize]
        public async Task<ActionResult<string>> SignDocument([FromBody] SignDocumentRequest signDocumentRequest)
        {
            var reportFile = await _reportsHandler.GetProtocolReport(signDocumentRequest.ProtocolId);
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
                accessToken, userInfo.Accounts.First().BaseUri + "/restapi", userInfo.Accounts.First().AccountId, report, "http://localhost:3000/signvalidation", signDocumentRequest.State);

            await _reportsHandler.SignReport(signDocumentRequest.ProtocolId, result.Item1);
            return Ok(result.Item2);
        }

        private int GetUserId()
        {
            return int.Parse(User.Claims.FirstOrDefault(x => x.Type == Claims.UserId).Value);
        }
    }
}
