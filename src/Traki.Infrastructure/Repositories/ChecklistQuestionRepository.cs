﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Infrastructure.Entities;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Domain.Models;
using Traki.Domain.Extensions;

namespace Traki.Infrastructure.Repositories
{
    public class ChecklistQuestionRepository : IChecklistQuestionRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public ChecklistQuestionRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddChecklistQuestions(IEnumerable<ChecklistQuestion> checklistQuestions)
        {
            var checkListQuestionEntities = _mapper.Map<IEnumerable<ChecklistQuestionEntity>>(checklistQuestions);
            await _context.CheckListQuestions.AddRangeAsync(checkListQuestionEntities);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChecklistQuestion>> GetChecklistQuestions(int checklistId)
        {
            var checklist = await _context.OldChecklists
                .Where(x => x.Id == checklistId)
                .Include(x => x.ChecklistQuestions)
                .FirstOrDefaultAsync();

            checklist.RequiresToBeNotNullEnity();

            var checklistQuestions = checklist.ChecklistQuestions.ToList();
            return _mapper.Map<IEnumerable<ChecklistQuestion>>(checklistQuestions);
        }

        public async Task UpdateChecklistQuestions(IEnumerable<ChecklistQuestion> checklistQuestions)
        {
            foreach (var checklistQuestion in checklistQuestions)
            {
                var checklistQuestionUpdate = await _context.CheckListQuestions
                    .FirstOrDefaultAsync(x => x.Id == checklistQuestion.Id);

                checklistQuestionUpdate.RequiresToBeNotNullEnity();

                checklistQuestionUpdate.Comment = checklistQuestion.Comment;
                checklistQuestionUpdate.Evaluation = checklistQuestion.Evaluation;
            }
            await _context.SaveChangesAsync();
        }
    }
}
