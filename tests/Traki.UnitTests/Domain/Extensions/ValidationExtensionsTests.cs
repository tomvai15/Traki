using FluentAssertions;
using Traki.Domain.Exceptions;
using Traki.Domain.Extensions;

namespace Traki.UnitTests.Domain.Extensions
{
    public class ValidationExtensionsTests
    {
        [Fact]
        public void RequiresToBeNotNullEnity_EntityIsNull_ThrowsException()
        {
            // Arrange
            object entity = null;
            var exception = new EntityNotFoundException();

            // Act
            Action action = () => entity.RequiresToBeNotNullEnity(exception);

            // Assert
            action.Should().Throw<EntityNotFoundException>()
                .WithMessage(exception.Message);
        }

        [Fact]
        public void RequiresToBeNotNullEnity_EntityIsNotNull_NoExceptionThrown()
        {
            // Arrange
            object entity = new object();

            // Act
            Action action = () => entity.RequiresToBeNotNullEnity();

            // Assert
            action.Should().NotThrow();
        }
    }
}
