using Traki.Domain.Models.Drawing;

namespace Traki.Infrastructure.Entities.Drawing
{
    public class StatusChangeEntity
    {
        public int Id { get; set; }
        public DefectStatus From { get; set; }
        public DefectStatus To { get; set; }
        public string Date { get; set; }
        public int AuthorId { get; set; }
        public UserEntity Author { get; set; }
        public int DefectId { get; set; }
        public DefectEntity Defect { get; set; }
    }
}
