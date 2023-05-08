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
    public class SectionRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public SectionRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task CreateSection_ShouldCreateSection()
        {
            var section = new Section { Name = Any<string>(), Priority = 1, ProtocolId = 1};

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new SectionRepository(context, _mapper);

            var result = await repository.CreateSection(section);

            result.Name.Should().Be(section.Name);
            result.Priority.Should().Be(section.Priority);
            result.ProtocolId.Should().Be(section.ProtocolId);
        }

        [Fact]
        public async Task UpdateSection_ShouldUpdateSection()
        {
            var section = new Section { Name = Any<string>(), Priority = 1, ProtocolId = 1 };
            var sectionToUpdate = new SectionEntity { Name = Any<string>(), Priority = 1, ProtocolId = 1 };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            context.Sections.Add(sectionToUpdate);
            context.SaveChanges();
            section.Id = sectionToUpdate.Id;
            var repository = new SectionRepository(context, _mapper);

            var result = await repository.UpdateSection(section);

            var expectedSection = context.Sections.FirstOrDefault(x=> x.Id == sectionToUpdate.Id);
            expectedSection.Name.Should().Be(section.Name);
            expectedSection.Priority.Should().Be(section.Priority);
            expectedSection.ProtocolId.Should().Be(section.ProtocolId);
        }

        [Fact]
        public async Task DeleteSection_SectionExists_ShouldDeleteSection()
        {
            var section = new Section { Name = Any<string>(), Priority = 1, ProtocolId = 1 };
            var sectionToDelete = new SectionEntity { Name = Any<string>(), Priority = 1, ProtocolId = 1 };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            context.Sections.Add(sectionToDelete);
            context.SaveChanges();
            section.Id = sectionToDelete.Id;
            var repository = new SectionRepository(context, _mapper);

            await repository.DeleteSection(section);

            var expectedSection = context.Sections.FirstOrDefault(x => x.Id == section.Id);
            expectedSection.Should().Be(null);
        }

        [Fact]
        public async Task GetSection_ShouldReturnSection()
        {
            var section = new SectionEntity { Name = Any<string>(), Priority = 1, ProtocolId = 1 };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            context.Sections.Add(section);
            context.SaveChanges();
            var repository = new SectionRepository(context, _mapper);

            var result = await repository.GetSection(section.Id);

            result.Name.Should().Be(section.Name);
            result.Priority.Should().Be(section.Priority);
            result.ProtocolId.Should().Be(section.ProtocolId);
        }

        [Fact]
        public async Task GetSections_ShouldReturnSections()
        {
            int protocolId = 2;
            var expectedSections = ExampleData.Sections.Where(x => x.ProtocolId == protocolId);

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new SectionRepository(context, _mapper);

            var result = await repository.GetSections(protocolId);

            expectedSections.Should().BeEquivalentTo(result, options => options.Excluding(x=> x.Checklist)
                .Excluding(x=> x.Table)
                .Excluding(x=> x.Id));
        }
    }
}
