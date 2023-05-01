using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Company;
using Traki.Api.Contracts.Recommendation;
using Traki.Domain.Handlers;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/recommendations")]
    [ApiController]
    [Authorize]
    public class RecommendationsController: ControllerBase
    {
        private readonly IRecommendationsHandler _recommendationsHandler;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IMapper _mapper;

        public RecommendationsController(IRecommendationsHandler recommendationsHandler, IClaimsProvider claimsProvider, IMapper mapper)
        {
            _recommendationsHandler = recommendationsHandler;
            _claimsProvider = claimsProvider;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetRecommendationResponse>> GetRecommendations()
        {
            _claimsProvider.TryGetUserId(out int userId);
            var reccomendation = await _recommendationsHandler.GetRecommendation(userId);

            var response = new GetRecommendationResponse
            {
                Recommendation = _mapper.Map<RecommendationDto>(reccomendation)
            };
            return Ok(response);
        }
    }
}
