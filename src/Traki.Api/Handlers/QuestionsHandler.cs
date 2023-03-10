﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Api.Contracts.Template;
using Traki.Api.Data;
using Traki.Api.Entities;
using Traki.Api.Extensions;
using Traki.Api.Models;

namespace Traki.Api.Handlers
{
    public interface IQuestionsHandler
    {
        Task<Question> GetQuestion(int templateId, int questionId);
        Task<IEnumerable<Question>> GetQuestions(int templateId);
        Task UpdateQuestion(int questionId, Question questionUpdate);
    }

    public class QuestionsHandler : IQuestionsHandler
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public QuestionsHandler(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Question> GetQuestion(int templateId, int questionId)
        {
            var question = _context.Questions.Where(x => x.Id == questionId).FirstOrDefault();

            question.RequiresNotNullEnity();

            return _mapper.Map<Question>(question);
        }

        public async Task<IEnumerable<Question>> GetQuestions(int templateId)
        {
            var template = await _context.Templates
                .Where(x => x.Id == templateId)
                .Include(x => x.Questions)
                .FirstOrDefaultAsync();

            template.RequiresNotNullEnity();


            var questions = template.Questions.ToList();
            return _mapper.Map<IEnumerable<Question>>(questions);
        }

        public async Task UpdateQuestion(int questionId, Question questionUpdate)
        {
            var question = _context.Questions.FirstOrDefault(x => x.Id == questionId);

            question.Title = questionUpdate.Title;
            question.Description = questionUpdate.Description;

            _context.SaveChangesAsync();
        }

        public void AssignNotNullProperties<T>(T source, T destination) where T : class
        {
            var p = source.GetType().GetProperties();
        }
    }
}
