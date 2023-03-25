namespace Traki.Infrastructure.Entities.Section.Items
{
    public class MultipleChoiceEntity
    {
        public string Id { get; set; }
        public IEnumerable<OptionEntity> Options { get; set; }
    }
}
