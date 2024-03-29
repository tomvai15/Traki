﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Protocol;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/protocols")]
    [Authorize]
    public class ProtocolsController : ControllerBase
    {
        private readonly IProtocolRepository _protocolRepository;
        private readonly ISectionHandler _sectionHandler;
        private readonly IMapper _mapper;

        public ProtocolsController(IProtocolRepository protocolRepository, ISectionHandler sectionHandler, IMapper mapper)
        {
            _protocolRepository = protocolRepository;
            _sectionHandler = sectionHandler;
            _mapper = mapper;
        }

        [HttpGet("{protocolId}")]
        public async Task<ActionResult<GetProtocolResponse>> GetProtocol(int protocolId)
        {
            var protocol = await _protocolRepository.GetProtocol(protocolId);

            var response = new GetProtocolResponse { Protocol = _mapper.Map<ProtocolDto>(protocol) };

            return Ok(response);
        }

        [HttpPut("{protocolId}")]
        public async Task<ActionResult> UpdateProtocol(int protocolId, [FromBody]UpdateProtocolRequest updateProtocolRequest)
        {
            var protocol = _mapper.Map<Protocol>(updateProtocolRequest.Protocol);
            var sections = _mapper.Map<IEnumerable<Section>>(updateProtocolRequest.Sections);
            await _protocolRepository.UpdateProtocol(protocol);
            await _sectionHandler.UpdateSections(sections);
            return Ok();
        }

        [HttpDelete("{protocolId}")]
        public async Task<ActionResult> DeleteProtocol(int protocolId)
        {
            await _protocolRepository.DeleteProtocol(protocolId);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProtocol([FromBody] CreateProtocolRequest createProtocolRequest)
        {
            var protocol = _mapper.Map<Protocol>(createProtocolRequest.Protocol);
            await _protocolRepository.CreateProtocol(protocol);
            return Ok();
        }

        [HttpGet("templates")]
        public async Task<ActionResult<GetProtocolsResponse>> GetTemplateProtocols()
        {
            var protocols = await _protocolRepository.GetTemplateProtocols();

            var response = new GetProtocolsResponse { Protocols = _mapper.Map<IEnumerable<ProtocolDto>>(protocols) };

            return Ok(response);
        }
    }
}
