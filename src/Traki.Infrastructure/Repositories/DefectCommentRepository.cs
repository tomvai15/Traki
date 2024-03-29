﻿using AutoMapper;
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

        public async Task<DefectComment> CreateDefectComment(DefectComment defectComment)
        {
            var defectCommentEntity = _mapper.Map<DefectCommentEntity>(defectComment);

            defectCommentEntity.Id = defectComment.Id;
            defectCommentEntity.AuthorId = defectComment.AuthorId;
            _context.Add(defectCommentEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<DefectComment>(defectCommentEntity);
        }
    }
}
