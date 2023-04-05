using AutoMapper;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Drawing;

namespace Traki.Infrastructure.Repositories
{
    public class DefectCommentRepository : IDefectCommentRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public DefectCommentRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateDefectComment(DefectComment defectComment)
        {
            var defectCommentEntity = _mapper.Map<DefectCommentEntity>(defectComment);

            defectCommentEntity.Id = defectComment.Id;
            defectCommentEntity.UserId = 1;
            _context.Add(defectCommentEntity);
            await _context.SaveChangesAsync();
        }
    }
}
