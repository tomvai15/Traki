﻿namespace Traki.Api.Contracts.Question
{
    public class GetQuestionsResponse
    {
        public IEnumerable<QuestionDto> Questions { get; set; }
    }
}