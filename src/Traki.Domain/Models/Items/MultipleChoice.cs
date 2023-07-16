namespace Traki.Domain.Models.Items
{
    public class MultipleChoice
    {
        public string Id { get; set; }
        public IEnumerable<Option> Options { get; set; }
    }
}
