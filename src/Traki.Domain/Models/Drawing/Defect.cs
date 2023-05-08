namespace Traki.Domain.Models.Drawing
{
    public class Defect
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageName { get; set; }
        public DefectStatus Status { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int DrawingId { get; set; }
        public Drawing Drawing { get; set; }
        public IEnumerable<DefectComment> DefectComments { get; set; }
        public IEnumerable<StatusChange> StatusChanges { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
    }
}
