namespace Traki.Domain.Models.Section.Items
{
    public class MultipleChoice
    {
        public string Id { get; set; }
        public IEnumerable<Option> Options { get; set; }
    }
}
