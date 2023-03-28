namespace Traki.Domain.Models.Drawing
{
    public class Defect
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
