namespace Traki.Api.Contracts.Report
{
    public class GenerateReportRequest
    {
        public string ReportTitle { get; set; }
        public bool UseColors { get; set; }
        public IEnumerable<int> SectionsToNotInclude { get; set; }
    }
}
