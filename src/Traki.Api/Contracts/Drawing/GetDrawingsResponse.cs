namespace Traki.Api.Contracts.Drawing
{
    public class GetDrawingsResponse
    {
        public IEnumerable<DrawingDto> Drawings { get; set; }
    }
}
