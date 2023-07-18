using Traki.Api.Contracts.Section.Items;

namespace Traki.Api.Contracts.Section
{
    public record ChecklistDto: SectionBaseDto
    {
        public List<MultipleChoiceDto> MultipleChoices { get; set; }
        public List<TextInputDto> TextInputs { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }
}
