namespace Traki.Api.Contracts.Section.Items
{
    public class ItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public QuestionDto? Question { get; set; }
        public MultipleChoiceDto? MultipleChoice { get; set; }
        public TextInputDto? TextInput { get; set; }
    }
}
