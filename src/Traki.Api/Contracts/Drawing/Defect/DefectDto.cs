using Traki.Domain.Models.Drawing;

namespace Traki.Api.Contracts.Drawing.Defect
{
    public class DefectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DefectStatus Status { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
