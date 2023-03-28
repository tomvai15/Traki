namespace Traki.Domain.Models.Drawing
{
    public class Drawing
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public IEnumerable<Defect> Defects { get; set; }
    }
}
