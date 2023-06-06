using Traki.Domain.Models.Drawing;

namespace Traki.Api.Contracts.Drawing.Defect
{
    public class DefectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DefectStatus Status { get; set; }
        public string ImageName { get; set; }
        public DateTime? CreationDate { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int DrawingId { get; set; }
        public IEnumerable<DefectCommentDto> DefectComments { get; set; }
        public IEnumerable<StatusChangeDto> StatusChanges { get; set; }
        public AuthorDto Author { get; set; }
    }
}
