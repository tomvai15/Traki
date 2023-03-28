using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Drawing;
using Traki.Api.Contracts.Product;
using Traki.Domain.Handlers;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/products/{productId}/drawings")]
    public class DrawingsController : ControllerBase
    {
        private readonly IDrawingsRepository _drawingsRepository;
        private readonly IMapper _mapper;

        public DrawingsController(IDrawingsRepository drawingsRepository, IMapper mapper)
        {
            _drawingsRepository = drawingsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetDrawingsResponse>> GetDrawings(int productId)
        {
            var drawings = await _drawingsRepository.GetDrawings(productId);

            var response = new GetDrawingsResponse
            {
                Drawings = _mapper.Map<IEnumerable<DrawingDto>>(drawings)
            };
            return Ok(response);
        }
    }
}
