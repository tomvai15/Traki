using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Report;
using Traki.Domain.Handlers;

namespace Traki.Api.Controllers
{
    [Route("api/protocols/{protocolId}/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportHandler _reportsHandler;
        private readonly IDocumentSignerHandler _documentSignerHandler;

        public ReportsController(IReportHandler reportsHandler, IDocumentSignerHandler documentSignerHandler)
        {
            _reportsHandler = reportsHandler;
            _documentSignerHandler = documentSignerHandler;
        }

        [HttpGet("raw")]
        [Authorize]
        public async Task<ActionResult> GetReportRaw(int protocolId)
        {
            var report = await _reportsHandler.GetProtocolReport(protocolId);

            return File(report, "application/pdf");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetReportResponse>> GetReport(int protocolId)
        {
            try
            {
                var report = await _reportsHandler.GetProtocolReport(protocolId);
                var reportBase64 = Convert.ToBase64String(report);
                return new GetReportResponse { Exists = true, ReportBase64 = reportBase64 };
            }
            catch (Exception ex)
            {
                return new GetReportResponse { Exists = false};
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> GenerateReport(int protocolId, [FromBody] GenerateReportRequest request)
        {
            await _reportsHandler.GenerateProtocolReport(protocolId, request.ReportTitle, request.UseColors, request.SectionsToNotInclude);
            return Ok();
        }

        [HttpGet("validate")]
        [Authorize]
        public async Task<ActionResult> ValidateDocumentSign(int protocolId)
        {
            await _documentSignerHandler.ValidateSign(protocolId);
            return Ok();
        }

        [HttpPost("sign")]
        [Authorize]
        public async Task<ActionResult<string>> SignDocument([FromBody] SignDocumentRequest signDocumentRequest)
        {
            string redirectUri = await _documentSignerHandler.SignDocument(signDocumentRequest.ProtocolId, signDocumentRequest.State);
            return Ok(redirectUri);
        }
    }
}
