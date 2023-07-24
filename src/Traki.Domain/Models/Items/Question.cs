namespace Traki.Domain.Models.Items
{
    public class Question: IItemContent
    {
        public string Comment { get; set; }
        public AnswerType? Answer { get; set; }
    }
}
