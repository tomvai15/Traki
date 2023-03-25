using AutoMapper;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;

namespace Traki.Domain.Handlers
{
    public interface ISectionHandler
    {
        Task AddOrUpdateSection(Section section);
    }

    public class SectionHandler : ISectionHandler
    {
        private readonly IMapper _mapper;
        private readonly ISectionRepository _sectionRepository;
        private readonly IChecklistRepository _checklistRepository;

        public SectionHandler(IMapper mapper, ISectionRepository sectionRepository, IChecklistRepository checklistRepository)
        {
            _mapper = mapper;
            _sectionRepository = sectionRepository;
            _checklistRepository = checklistRepository;
        }

        public async Task AddOrUpdateSection(Section section)
        {
            var sectionFromDatabase = _sectionRepository.GetSection(section.Id);

            var createdSection = section;
            if (sectionFromDatabase == null)
            {
                createdSection = await _sectionRepository.CreateSection(section);
            } 
            else
            {
                await _checklistRepository.DeleteChecklist(section.Checklist.Id);
            }

            var createdChecklist = await _checklistRepository.CreateChecklist(section.Checklist);


        }
    }
}
