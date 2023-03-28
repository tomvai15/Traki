namespace Traki.Api.Contracts.Protocol
{
    public class ProtocolDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ReportName { get; set; }
        public bool IsTemplate { get; set; }
    }
}
