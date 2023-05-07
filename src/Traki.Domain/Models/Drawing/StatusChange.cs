namespace Traki.Domain.Models.Drawing
{
    public class StatusChange
    {
        public int Id { get; set; }
        public DefectStatus From { get; set; }
        public DefectStatus To { get; set; }
        public string Date { get; set; }
        public int DefectId { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
    }
}
