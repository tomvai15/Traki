namespace Traki.Domain.Models.Items
{
    public class Question
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public AnswerType? Answer { get; set; }
    }
}
