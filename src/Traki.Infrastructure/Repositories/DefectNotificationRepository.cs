using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Drawing;

namespace Traki.Infrastructure.Repositories
{
    public class DefectNotificationRepository : IDefectNotificationRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public DefectNotificationRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task CreateDefectNotification(DefectNotification defectNotification)
        {
            var defectNotificationEntity = _mapper.Map<DefectNotificationEntity>(defectNotification);

            _context.DefectNotifications.Add(defectNotificationEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDefectNotification(int userId, int defectId)
        {
            var defectNotifications = await _context.DefectNotifications
                .Where(x => x.DefectId == defectId && x.UserId == userId)
                .ToListAsync();
            _context.DefectNotifications.RemoveRange(defectNotifications);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DefectNotification>> GetUserDefectNotifications(int userId)
        {
            var defectNotifications = await _context.DefectNotifications.Where(x => x.UserId == userId).ToListAsync();

            return _mapper.Map<IEnumerable<DefectNotification>>(defectNotifications);
        }
    }
}
