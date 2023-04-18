using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.Domain.Services.Notifications;

namespace Traki.Domain.Handlers
{
    public interface IDefectHandler
    {
        Task<Defect> CreateDefect(int userId, int drawingId, Defect defect);
    }
    public class DefectHandler : IDefectHandler
    {
        private readonly IDefectsRepository _defectsRepository;
        private readonly INotificationService _notificationService;
        private readonly IDrawingsRepository _drawingsRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IUsersRepository _usersRepository;

        public DefectHandler(IDefectsRepository defectsRepository, INotificationService notificationService, IDrawingsRepository drawingsRepository, IProductsRepository productsRepository, IUsersRepository usersRepository)
        {
            _defectsRepository = defectsRepository;
            _notificationService = notificationService;
            _drawingsRepository = drawingsRepository;
            _productsRepository = productsRepository;
            _usersRepository = usersRepository;
        }

        public async Task<Defect> CreateDefect(int userId, int drawingId, Defect defect)
        {
            defect.UserId = userId;
            defect.DrawingId = drawingId;
            defect.Status = DefectStatus.NotFixed;
            defect = await _defectsRepository.CreateDefect(defect);

            var drawing = await _drawingsRepository.GetDrawing(drawingId);

            var product = await _productsRepository.GetProduct(drawing.ProductId);

            var user = await _usersRepository.GetUserById(product.UserId);


            // todo create new component INotificationHandler
            string deviceToken = user.DeviceToken;
            if (deviceToken.IsNullOrEmpty())
            {
                return defect;
            }

            await _notificationService.SendNotification(deviceToken, "New defect", "new defect was created");
            return defect;
        }
    }
}
