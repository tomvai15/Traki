namespace Traki.Infrastructure.Entities.Section
{
    public class TableEntity
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public SectionEntity Section { get; set; }
    }
}
