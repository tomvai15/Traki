namespace Traki.Api.Contracts.Drawing.Defect
{
    public class DefectCommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ImageName { get; set; }
        public string Date { get; set; }
        public AuthorDto Author { get; set; }
    }
}
