using Traki.Domain.Models.Drawing;

namespace Traki.Domain.Models
{
    public class DefectRecomendation
    {
        public Defect Defect { get; set; }
        public string ProductName { get; set; }
        public int ProjectId { get; set; }
        public int ProductId { get; set; }
    }
}
