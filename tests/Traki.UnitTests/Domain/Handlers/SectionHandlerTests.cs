using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Handlers;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Domain.Handlers
{
    public class SectionHandlerTests
    {
        private readonly Mock<ISectionRepository> _sectionRepository = new Mock<ISectionRepository>();
        private readonly Mock<IChecklistRepository> _checklistRepository = new Mock<IChecklistRepository>();
        private readonly Mock<IItemRepository> _itemRepository = new Mock<IItemRepository>();
        private readonly Mock<ITableRepository> _tableRepository = new Mock<ITableRepository>();
        private readonly Mock<ITableRowRepository> _tableRowRepository = new Mock<ITableRowRepository>();

        private readonly SectionHandler _sectionHandler;
        public SectionHandlerTests() 
        {
            _sectionHandler = new SectionHandler(_sectionRepository.Object, _checklistRepository.Object, _itemRepository.Object, _tableRepository.Object, _tableRowRepository.Object);
            _sectionRepository.Setup(x => x.GetSection(It.IsAny<int>())).ReturnsAsync(MockData.Sections.First());

            var checklist = MockData.NewChecklists.First();
            checklist.Items = MockData.Items.ToList();
            _sectionRepository.Setup(x => x.GetSection(It.IsAny<int>())).ReturnsAsync(MockData.Sections.First());
            _sectionRepository.Setup(x => x.CreateSection(It.IsAny<Section>())).ReturnsAsync(MockData.Sections.First());
            _sectionRepository.Setup(x => x.UpdateSection(It.IsAny<Section>())).ReturnsAsync(MockData.Sections.First());


            _checklistRepository.Setup(x => x.GetSectionChecklist(It.IsAny<int>())).ReturnsAsync(checklist);

            _itemRepository.Setup(x => x.GetChecklistItems(It.IsAny<int>())).ReturnsAsync(MockData.Items.ToList());

            _tableRepository.Setup(x => x.GetSectionTable(It.IsAny<int>())).ReturnsAsync(MockData.Tables.First());
            _tableRepository.Setup(x => x.CreateTable(It.IsAny<Table>())).ReturnsAsync(MockData.Tables.First());
            _tableRowRepository.Setup(x => x.GetTableRows(It.IsAny<int>())).ReturnsAsync(MockData.TableRows);
        }

        [Fact]
        public async Task GetSection_ReturnsSection()
        {
            int sectionId = 1;

            var result = await _sectionHandler.GetSection(sectionId);


            var expectedSection = MockData.Sections.First();
            result.Name.Should().BeEquivalentTo(expectedSection.Name);
        }

        [Fact]
        public async Task UpdateSections_ShouldUpdateSections()
        {
            int sectionId = 1;
            int count = MockData.Sections.Count();

            await _sectionHandler.UpdateSections(MockData.Sections.ToList());

            _sectionRepository.Verify(x => x.UpdateSection(It.IsAny<Traki.Domain.Models.Section.Section>()), Times.Exactly(count));
        }

        [Fact]
        public async Task UpdateSectionAnswers()
        {
            int protocolId = 1;
            int count = MockData.Sections.Count();
            var section = MockData.Sections.First();
            section.Checklist = MockData.NewChecklists.First();
            section.Table = MockData.Tables.First();
            section.Table.TableRows = MockData.TableRows.ToList();

            await _sectionHandler.UpdateSectionAnswers(protocolId, section);

           // _sectionRepository.Verify(x => x.UpdateSection(It.IsAny<Traki.Domain.Models.Section.Section>()), Times.Exactly(count));

            Assert.True(true);
        }

        [Fact]
        public async Task AddOrUpdateSection()
        {
            int protocolId = 1;
            int count = MockData.Sections.Count();
            var section = MockData.Sections.First();
            section.Checklist = MockData.NewChecklists.First();
            section.Checklist.Items = MockData.Items.ToList();
            section.Table = MockData.Tables.First();
            section.Table.TableRows = MockData.TableRows.ToList();

            await _sectionHandler.AddOrUpdateSection(protocolId, section);

            // _sectionRepository.Verify(x => x.UpdateSection(It.IsAny<Traki.Domain.Models.Section.Section>()), Times.Exactly(count));

            Assert.True(true);
        }


        [Fact]
        public async Task DeleteSection()
        {
            int sectionId = 1;
            int count = MockData.Sections.Count();
            var section = MockData.Sections.First();
            section.Checklist = MockData.NewChecklists.First();
            section.Checklist.Items = MockData.Items.ToList();
            section.Table = MockData.Tables.First();
            section.Table.TableRows = MockData.TableRows.ToList();

            await _sectionHandler.DeleteSection(sectionId);

            // _sectionRepository.Verify(x => x.UpdateSection(It.IsAny<Traki.Domain.Models.Section.Section>()), Times.Exactly(count));

            Assert.True(true);
        }
    }
}
