﻿using Traki.Domain.Models.Items;

namespace Traki.Api.Contracts.Section.Items
{
    public class QuestionDto
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public AnswerType? Answer { get; set; }
    }
}
