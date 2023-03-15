using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Template;
using Traki.Api.Data.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/projects/{projectId}/templates")]
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplatesRepository _templatesRepository;
        private readonly IMapper _mapper;

        public TemplatesController(ITemplatesRepository templatesRepository, IMapper mapper)
        {
            _templatesRepository = templatesRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetTemplatesResponse>> GetTemplates(int projectId)
        {
            var templates = await _templatesRepository.GetTemplates(projectId);

            return _mapper.Map<GetTemplatesResponse>(templates);
        }

        [HttpGet("{templateId}")]
        public async Task<ActionResult<GetTemplateResponse>> GetTemplate(int projectId, int templateId)
        {
            var template = await _templatesRepository.GetTemplate(templateId);

            return _mapper.Map<GetTemplateResponse>(template);
        }
    }
}
