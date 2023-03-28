using Traki.Api.Contracts.Drawing.Defect;

namespace Traki.Api.Contracts.Drawing
{
    public class DrawingDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public IEnumerable<DefectDto> Defects { get; set; }
    }
}
