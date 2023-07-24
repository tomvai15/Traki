﻿using Traki.Domain.Models.Items;

namespace Traki.Api.Contracts.Section.Items
{
    public record QuestionDto
    {
        public string Comment { get; set; }
        public AnswerType? Answer { get; set; }
    }
}
