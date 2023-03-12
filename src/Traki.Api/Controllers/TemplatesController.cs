﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Template;
using Traki.Api.Handlers;

namespace Traki.Api.Controllers
{
    [Route("api/projects/{projectId}/templates")]
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplatesHandler _templatesHandler;
        private readonly IMapper _mapper;

        public TemplatesController(ITemplatesHandler templatesHandler, IMapper mapper)
        {
            _templatesHandler = templatesHandler;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetTemplatesResponse>> GetTemplates(int projectId)
        {
            var templates = await _templatesHandler.GetTemplates(projectId);

            return _mapper.Map<GetTemplatesResponse>(templates);
        }

        [HttpGet("{templateId}")]
        public async Task<ActionResult<GetTemplateResponse>> GetTemplate(int projectId, int templateId)
        {
            var template = await _templatesHandler.GetTemplate(projectId, templateId);

            return _mapper.Map<GetTemplateResponse>(template);
        }
    }
}