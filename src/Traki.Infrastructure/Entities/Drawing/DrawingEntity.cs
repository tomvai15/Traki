namespace Traki.Infrastructure.Entities.Drawing
{
    public class DrawingEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public IEnumerable<DefectEntity> Defects { get; set; }
    }
}
