namespace Traki.Infrastructure.Entities.Section.Items
{
    public class OptionEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public MultipleChoiceEntity MultipleChoice { get; set; }
    }
}
