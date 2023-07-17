using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Section;
using Traki.Domain.Models;

namespace Traki.Infrastructure.Repositories
{
    public class ChecklistRepository : IChecklistRepository
    {
        public Task<Checklist> CreateChecklist(Checklist checkList)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteChecklist(int checklistId)
        {
            throw new NotImplementedException();
        }

        public Task<Checklist> GetChecklist(int checklistId)
        {
            throw new NotImplementedException();
        }

        public Task<Checklist> GetSectionChecklist(int sectionId)
        {
            throw new NotImplementedException();
        }
    }
}
