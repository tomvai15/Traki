using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Repositories;

namespace Traki.UnitTests.Domain.Handlers
{
    public class ChecklistHandlerTests
    {
        private readonly Mock<IQuestionsRepository> _mockQuestionsRepository;
        private readonly Mock<ITemplatesRepository> _mockTemplatesRepository;
        private readonly Mock<IChecklistRepository> _mockChecklistRepository;
        private readonly Mock<IChecklistQuestionRepository> _mockChecklistQuestionRepository;
        private readonly ChecklistHandler _checklistHandler;

        public ChecklistHandlerTests()
        {
            _mockQuestionsRepository = new Mock<IQuestionsRepository>();
            _mockTemplatesRepository = new Mock<ITemplatesRepository>();
            _mockChecklistRepository = new Mock<IChecklistRepository>();
            _mockChecklistQuestionRepository = new Mock<IChecklistQuestionRepository>();

            _checklistHandler = new ChecklistHandler(
                _mockChecklistRepository.Object,
                _mockChecklistQuestionRepository.Object,
                _mockQuestionsRepository.Object,
                _mockTemplatesRepository.Object);
        }

        [Fact]
        public async Task CreateChecklistFromTemplate_ShouldCreateChecklistAndQuestions()
        {
            // Arrange
            var productId = 1;
            var templateId = 2;
            var questions = new[]
            {
            new Question { Title = "Question 1", Description = "Description 1" },
            new Question { Title = "Question 2", Description = "Description 2" },
        };
            var template = new Template { Id = templateId, Name = "Template 1", Standard = "Standard 1" };
            var addedChecklist = new CheckList { Id = 3, ProductId = productId, Name = template.Name, Standard = template.Standard };

            _mockQuestionsRepository.Setup(x => x.GetQuestions(templateId)).ReturnsAsync(questions);
            _mockTemplatesRepository.Setup(x => x.GetTemplate(templateId)).ReturnsAsync(template);
            _mockChecklistRepository.Setup(x => x.AddChecklist(It.IsAny<CheckList>())).ReturnsAsync(addedChecklist);

            // Act
            await _checklistHandler.CreateChecklistFromTemplate(productId, templateId);

            // Assert
            _mockQuestionsRepository.Verify(x => x.GetQuestions(templateId), Times.Once);
            _mockTemplatesRepository.Verify(x => x.GetTemplate(templateId), Times.Once);
            _mockChecklistRepository.Verify(x => x.AddChecklist(It.Is<CheckList>(c => c.ProductId == productId && c.Name == template.Name && c.Standard == template.Standard)), Times.Once);
            _mockChecklistQuestionRepository.Verify(x => x.AddChecklistQuestions(It.Is<IEnumerable<ChecklistQuestion>>(cq => cq.Count() == questions.Length && cq.All(q => q.ChecklistId == addedChecklist.Id && q.Comment == string.Empty && q.Evaluation == Evaluation.No))), Times.Once);
        }
    }
}
