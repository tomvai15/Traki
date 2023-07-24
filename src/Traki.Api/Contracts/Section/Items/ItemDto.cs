namespace Traki.Api.Contracts.Section.Items
{
    public abstract record ItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ItemImage { get; set; }
        public int Index { get; set; }
        public QuestionDto? Question { get; set; }
        public MultipleChoiceDto? MultipleChoice { get; set; }
        public TextInputDto? TextInput { get; set; }
    }
}
