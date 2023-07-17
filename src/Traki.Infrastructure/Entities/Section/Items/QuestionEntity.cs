using Traki.Domain.Models.Items;

namespace Traki.Infrastructure.Entities.Section.Items
{
    public class QuestionEntity: ItemBase
    {
        public string Comment { get; set; }
        public AnswerType? Answer { get; set; }
    }
}
