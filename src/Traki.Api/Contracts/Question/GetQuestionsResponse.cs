namespace Traki.Api.Contracts.Question
{
    public class GetQuestionsResponse
    {
        public IEnumerable<OldQuestionDto> Questions { get; set; }
    }
}
