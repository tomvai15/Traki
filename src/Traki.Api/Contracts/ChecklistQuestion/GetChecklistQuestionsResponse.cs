namespace Traki.Api.Contracts.ChecklistQuestion
{
    public class GetChecklistQuestionsResponse
    {
        public IEnumerable<ChecklistQuestionDto> ChecklistQuestions { get; set; }
    }
}
