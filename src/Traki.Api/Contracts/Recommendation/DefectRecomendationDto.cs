using Traki.Api.Contracts.Drawing.Defect;

namespace Traki.Api.Contracts.Recommendation
{
    public class DefectRecomendationDto
    {
        public DefectDto Defect { get; set; }
        public int ProjectId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
