namespace Traki.Api.Contracts.Report
{
    public class GetReportResponse
    {
        public bool Exists { get; set; }
        public string ReportBase64 { get; set; }
    }
}
