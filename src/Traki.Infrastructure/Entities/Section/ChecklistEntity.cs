using Traki.Infrastructure.Entities.Section.Items;

namespace Traki.Infrastructure.Entities.Section
{
    public class ChecklistEntity: SectionBase
    {
        public ICollection<QuestionEntity> Questions { get; set; }
        public ICollection<MultipleChoiceEntity> MultipleChoices { get; set; }
        public ICollection<TextInputEntity> TextInputs { get; set; }
    }
}
