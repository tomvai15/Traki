using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Protocol;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;
using Traki.Domain.Services;

namespace Traki.Api.Controllers
{
    [Route("api/protocols")]
    public class ProtocolsController : ControllerBase
    {
        private readonly IProtocolRepository _protocolRepository;
        private readonly ISectionHandler _sectionHandler;
        private readonly IProtocolService _protocolService;
        private readonly IMapper _mapper;

        public ProtocolsController(IProtocolRepository protocolRepository, ISectionHandler sectionHandler, IProtocolService protocolService, IMapper mapper)
        {
            _protocolRepository = protocolRepository;
            _sectionHandler = sectionHandler;
            _protocolService = protocolService;
            _mapper = mapper;
        }

        [HttpGet("{protocolId}")]
        public async Task<ActionResult<GetProtocolResponse>> GetProtocol(int protocolId)
        {
            var protocol = await _protocolService.FindProtocol(protocolId);

            var response = new GetProtocolResponse { Protocol = _mapper.Map<ProtocolDto>(protocol) };

            return Ok(response);
        }

        [HttpPut("{protocolId}")]
        public async Task<ActionResult> UpdateProtocol(int protocolId, [FromBody]UpdateProtocolRequest updateProtocolRequest)
        {
            var protocol = _mapper.Map<Protocol>(updateProtocolRequest.Protocol);
            await _protocolService.UpdateProtocol(protocol);
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
            await _protocolService.CreateProtocol(protocol);
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
