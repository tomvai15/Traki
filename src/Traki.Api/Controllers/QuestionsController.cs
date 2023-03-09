using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Question;
using Traki.Api.Handlers;

namespace Traki.Api.Controllers
{
    [Route("api/templates/{templateId}/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionsHandler _questionsHandler;
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionsHandler questionsHandler, IMapper mapper)
        {
            _questionsHandler = questionsHandler;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetQuestionsResponse>> GetQuestions(int templateId)
        {
            var questions = await _questionsHandler.GetQuestions(templateId);

            return _mapper.Map<GetQuestionsResponse>(questions);
        }
    }
}
