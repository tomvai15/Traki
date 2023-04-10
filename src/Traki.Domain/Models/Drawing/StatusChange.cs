using Traki.Domain.Models.Drawing;

namespace Traki.Infrastructure.Entities.Drawing
{
    public class StatusChange
    {
        public int Id { get; set; }
        public DefectStatus From { get; set; }
        public DefectStatus To { get; set; }
        public string Date { get; set; }
        public int UserId { get; set; }
        public int DefectId { get; set; }
    }
}
