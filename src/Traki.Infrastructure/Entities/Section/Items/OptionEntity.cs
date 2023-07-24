namespace Traki.Infrastructure.Entities.Section.Items
{
    public class OptionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public int MultipleChoiceId { get; set; }
        public MultipleChoiceEntity MultipleChoice { get; set; }
    }
}
