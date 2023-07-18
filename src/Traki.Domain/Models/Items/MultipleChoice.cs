namespace Traki.Domain.Models.Items
{
    public class MultipleChoice: Item
    {
        public IEnumerable<Option> Options { get; set; }
    }
}
