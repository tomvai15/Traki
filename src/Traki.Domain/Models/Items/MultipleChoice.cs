namespace Traki.Domain.Models.Items
{
    public class MultipleChoice: IItemContent
    {
        public IEnumerable<Option> Options { get; set; }
    }
}
