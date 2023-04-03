namespace Traki.Domain.Models.Drawing
{
    public class DefectComment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public string ImageName { get; set; }
        public string Author { get; set; }
        public int DefectId { get; set; }
    }
}
