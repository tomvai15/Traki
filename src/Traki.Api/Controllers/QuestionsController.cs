using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Question;
using Traki.Api.Models;
using Traki.Api.Data.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/templates/{templateId}/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionsRepository _questionsRepository;
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionsRepository questionsRepository, IMapper mapper)
        {
            _questionsRepository = questionsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetQuestionsResponse>> GetQuestions(int templateId)
        {
            var questions = await _questionsRepository.GetQuestions(templateId);

            return _mapper.Map<GetQuestionsResponse>(questions);
        }

        [HttpGet("{questionId}")]
        public async Task<ActionResult<GetQuestionResponse>> GetQuestion(int templateId, int questionId)
        {
            var question = await _questionsRepository.GetQuestion(templateId, questionId);

            return _mapper.Map<GetQuestionResponse>(question);
        }

        [HttpPut("{questionId}")]
        public async Task<ActionResult> UpdateQuestion(int templateId, int questionId, [FromBody]UpdateQuestionRequest updateQuestionRequest)
        {
            var questionUpdate = _mapper.Map<Question>(updateQuestionRequest);
            await _questionsRepository.UpdateQuestion(questionId, questionUpdate);

            return Ok();
        }
    }
}
