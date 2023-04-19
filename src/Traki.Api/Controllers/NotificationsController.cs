using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Api.Contracts.User;
using Traki.Domain.Handlers;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IDefectNotificationRepository _defectNotificationRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IMapper _mapper;

        public NotificationsController(IDefectNotificationRepository defectNotificationRepository, IClaimsProvider claimsProvider, IMapper mapper)
        {
            _defectNotificationRepository = defectNotificationRepository;
            _claimsProvider = claimsProvider;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetDefectNotificationsResponse>> GetNotifications()
        {
            _claimsProvider.TryGetUserId(out int userId);
            var defectNotifications = await _defectNotificationRepository.GetUserDefectNotifications(userId);
            var response = new GetDefectNotificationsResponse
            {
                DefectNotifications = _mapper.Map<IEnumerable<DefectNotificationDto>>(defectNotifications)
            };
            return Ok(response);
        }

        [HttpDelete("{defectId}")]
        [Authorize]
        public async Task<ActionResult> DeleteNotification(int defectId)
        {
            _claimsProvider.TryGetUserId(out int userId);
            await _defectNotificationRepository.DeleteDefectNotification(userId, defectId);
            return Ok();
        }
    }
}
