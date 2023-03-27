using Traki.Domain.Models.Section.Items;

namespace Traki.Infrastructure.Entities.Section.Items
{
    public class QuestionEntity
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public AnswerType? Answer { get; set; }
        public string ItemId { get; set; }
        public ItemEntity Item { get; set; }
    }
}
