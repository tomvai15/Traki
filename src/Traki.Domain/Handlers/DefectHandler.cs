using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Traki.Domain.Exceptions;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.Domain.Services.Notifications;

namespace Traki.Domain.Handlers
{
    public interface IDefectHandler
    {
        Task<Defect> CreateDefect(int userId, int drawingId, Defect defect);
        Task<Defect> CreateDefectStatusChange(int userId, Defect defect);
        Task CreateDefectComment(int userId, DefectComment defectComment);
    }
    public class DefectHandler : IDefectHandler
    {
        private readonly IDefectsRepository _defectsRepository;
        private readonly INotificationService _notificationService;
        private readonly IDrawingsRepository _drawingsRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IDefectNotificationRepository _defectNotificationRepository;
        private readonly IStatusChangeRepository _statusChangeRepository;
        private readonly IDefectCommentRepository _defectCommentRepository;

        public DefectHandler(IDefectCommentRepository defectCommentRepository, IStatusChangeRepository statusChangeRepository, IDefectsRepository defectsRepository, INotificationService notificationService, IDrawingsRepository drawingsRepository, IProductsRepository productsRepository, IUsersRepository usersRepository, IDefectNotificationRepository defectNotificationRepository)
        {
            _defectCommentRepository = defectCommentRepository;
            _statusChangeRepository = statusChangeRepository;
            _defectsRepository = defectsRepository;
            _notificationService = notificationService;
            _drawingsRepository = drawingsRepository;
            _productsRepository = productsRepository;
            _usersRepository = usersRepository;
            _defectNotificationRepository = defectNotificationRepository;
        }

        public async Task<Defect> CreateDefect(int userId, int drawingId, Defect defect)
        {
            var defectAuthor = await _usersRepository.GetUserById(userId);
            defect.AuthorId = userId;
            defect.DrawingId = drawingId;
            defect.Status = DefectStatus.NotFixed;
            defect = await _defectsRepository.CreateDefect(defect);

            var drawing = await _drawingsRepository.GetDrawing(drawingId);
            var product = await _productsRepository.GetProduct(drawing.ProductId);
            var user = await _usersRepository.GetUserById(product.AuthorId);

            string data = JsonConvert.SerializeObject(new
            {
                projectId = product.ProjectId,
                productId = product.Id,
                drawingId = drawing.Id,
                defectId = defect.Id
            });

            var defectNotification = new DefectNotification
            {
                DefectId = defect.Id,
                UserId = product.AuthorId,
                Title = "New defect",
                Body = $"{defectAuthor.Name} {defectAuthor.Surname} create new defect for product {product.Name}",
                Data = data
            };

            await _defectNotificationRepository.CreateDefectNotification(defectNotification);

            string deviceToken = user.DeviceToken;
            if (string.IsNullOrEmpty(deviceToken))
            {
                return defect;
            }

            await _notificationService.SendNotification(deviceToken, defectNotification.Title, defectNotification.Body);
            return defect;
        }

        public async Task CreateDefectComment(int userId, DefectComment defectComment)
        {
            var defect = await _defectsRepository.GetDefect(defectComment.DefectId);
            defectComment.AuthorId = userId;
            defectComment.Date = DateTime.Now.ToString("s");
            await _defectCommentRepository.CreateDefectComment(defectComment);
            var commentAuthor = await _usersRepository.GetUserById(userId);

            string data = await CreateData(defect.Id, defect.DrawingId);

            var defectNotification = new DefectNotification
            {
                DefectId = defectComment.DefectId,
                UserId = defect.AuthorId,
                Title = "New comment",
                Body = $"{commentAuthor.Name} {commentAuthor.Surname} commented on defect {defect.Title}",
                Data = data
            };

            await _defectNotificationRepository.CreateDefectNotification(defectNotification);

            var author = await _usersRepository.GetUserById(defect.AuthorId);

            string deviceToken = author.DeviceToken;

            if (string.IsNullOrEmpty(deviceToken))
            {
                return;
            }
            await _notificationService.SendNotification(deviceToken, defectNotification.Title, defectNotification.Body);
        }

        public async Task<Defect> CreateDefectStatusChange(int userId, Defect defect)
        {
            var def = await _defectsRepository.GetDefect(defect.Id);

            if (def.Status == defect.Status)
            {
                throw new BadOperationException("Cannot change status to the same");
            }

            if (userId != def.AuthorId && def.Status != DefectStatus.NotFixed)
            {
                throw new ForbbidenOperationException("Only defect author can change defect status, when it's not in Fixed state");
            }

            var statusChange = new StatusChange
            {
                From = def.Status,
                To = defect.Status,
                Date = DateTime.Now.ToString("s"),
                AuthorId = userId,
                DefectId = defect.Id,
            };

            defect = await _defectsRepository.UpdateDefect(defect);
            await _statusChangeRepository.CreateStatusChange(statusChange);

            string data = await CreateData(defect.Id, defect.DrawingId);
            var statusChangeAuthor = await _usersRepository.GetUserById(userId);

            var defectNotification = new DefectNotification
            {
                DefectId = defect.Id,
                UserId = def.AuthorId,
                Title = "Defect status change",
                Body = $"{statusChangeAuthor.Name} {statusChangeAuthor.Surname} changed status for defect {defect.Title} to {statusChange.To}",
                Data = data
            };

            await _defectNotificationRepository.CreateDefectNotification(defectNotification);

            var author = await _usersRepository.GetUserById(defect.AuthorId);



            string deviceToken = author.DeviceToken;

            if (string.IsNullOrEmpty(deviceToken))
            {
                return defect;
            }
            await _notificationService.SendNotification(deviceToken, defectNotification.Title, defectNotification.Body);

            return defect;
        }

        private async Task<string> CreateData(int defectId, int drawingId)
        {
            var drawing = await _drawingsRepository.GetDrawing(drawingId);
            var product = await _productsRepository.GetProduct(drawing.ProductId);

            return JsonConvert.SerializeObject(new
            {
                projectId = product.ProjectId,
                productId = product.Id,
                drawingId = drawing.Id,
                defectId = defectId
            });
        }
    }
}
