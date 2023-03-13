namespace Traki.Api.Contracts.ChecklistQuestion
{
    public class UpdateChecklistQuestionsRequest
    {
        public IEnumerable<ChecklistQuestionDto> ChecklistQuestions { get; set; }
    }
}
