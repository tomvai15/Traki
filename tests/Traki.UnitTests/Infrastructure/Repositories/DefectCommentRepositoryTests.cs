using AutoMapper;
using FluentAssertions;
using Traki.Domain.Models.Drawing;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class DefectCommentRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public DefectCommentRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task CreateDefectComment_CreatesDefectComment()
        {
            var comment = new DefectComment
            {
                Text = Any<string>(),
                Date = Any<string>(),
                ImageName = Any<string>(),
                DefectId = 1,
                AuthorId = 1
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DefectCommentRepository(context, _mapper);

            var cratedComment = await repository.CreateDefectComment(comment);

            cratedComment.Text.Should().Be(comment.Text);
            cratedComment.ImageName.Should().Be(comment.ImageName);
            cratedComment.Date.Should().Be(comment.Date);
            cratedComment.DefectId.Should().Be(comment.DefectId);
            cratedComment.AuthorId.Should().Be(comment.AuthorId);
        }
    }
}
