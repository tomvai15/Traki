using AutoMapper;
using FluentAssertions;
using Traki.Domain.Models.Section;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class ChecklistRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public ChecklistRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task CreateChecklist_ShouldCreateChecklist()
        {
            var checklist = new Checklist { SectionId = 1 };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ChecklistRepository(context, _mapper);

            var result = await repository.CreateChecklist(checklist);

            result.SectionId.Should().Be(checklist.SectionId);
        }

        [Fact]
        public async Task DeleteChecklist_ChecklistExists_ShouldDeleteChecklistAndReturnTrue()
        {
            var checklistEntity = new ChecklistEntity{ SectionId = 1 };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);

            context.Checklists.Add(checklistEntity);
            context.SaveChanges();
            var repository = new ChecklistRepository(context, _mapper);

            var result = await repository.DeleteChecklist(checklistEntity.Id);

            result.Should().Be(true);
        }

        [Fact]
        public async Task DeleteChecklist_ChecklistDoesNotExists_ShouldReturnFalse()
        {
            int notExistingChecklistId = 10000;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ChecklistRepository(context, _mapper);

            var result = await repository.DeleteChecklist(notExistingChecklistId);

            result.Should().Be(false);
        }

        [Fact]
        public async Task GetChecklist_ShouldReturnChecklist()
        {
            var checklistEntity = new ChecklistEntity { SectionId = 1 };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            context.Checklists.Add(checklistEntity);
            context.SaveChanges();
            var repository = new ChecklistRepository(context, _mapper);

            var result = await repository.GetChecklist(checklistEntity.Id);

            result.SectionId.Should().Be(checklistEntity.SectionId);
        }

        [Fact]
        public async Task GetSectionChecklist_ShouldReturnSectionChecklist()
        {
            int sectionId = 1;
            var checklistEntity = new ChecklistEntity { SectionId = sectionId };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            context.Checklists.Add(checklistEntity);
            context.SaveChanges();
            var repository = new ChecklistRepository(context, _mapper);

            var result = await repository.GetSectionChecklist(sectionId);

            result.SectionId.Should().Be(checklistEntity.SectionId);
        }
    }
}
