namespace Traki.Api.Contracts.Section.Items
{
    public class MultipleChoiceDto
    {
        public string Id { get; set; }
        public IEnumerable<OptionDto> Values { get; set; }
    }
}
