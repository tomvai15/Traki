namespace Traki.Infrastructure.Entities.Section.Items
{
    public class MultipleChoiceEntity: ItemBase
    {
        public IEnumerable<OptionEntity> Options { get; set; }
    }
}
