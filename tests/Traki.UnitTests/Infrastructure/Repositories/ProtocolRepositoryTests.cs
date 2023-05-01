using AutoMapper;
using DocuSign.eSign.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;
using Product = Traki.Domain.Models.Product;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class ProtocolRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public ProtocolRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task GetTemplateProtocols_ReturnsProtocols()
        {
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProtocolRepository(context, _mapper);
            var expectedProducts = await context.Protocols.Where(p => p.IsTemplate == true).ToListAsync();

            var products = await repository.GetTemplateProtocols();

            expectedProducts.Should().BeEquivalentTo(products, options => options.Excluding(x => x.Signer));
        }

        [Fact]
        public async Task GetProtocols_ReturnsProtocols()
        {
            int productId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProtocolRepository(context, _mapper);
            var expectedProducts = await context.Protocols.Where(x => x.ProductId == productId).ToListAsync();

            var products = await repository.GetProtocols(productId);

            expectedProducts.Should().BeEquivalentTo(products, options => options.Excluding(x => x.Signer));
        }

        [Fact]
        public async Task GetProtocol_ReturnsProtocol()
        {
            int protocolId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProtocolRepository(context, _mapper);
            var expectedProduct = await context.Protocols.Where(x => x.Id == protocolId).FirstOrDefaultAsync();

            var product = await repository.GetProtocol(protocolId);

            expectedProduct.Should().BeEquivalentTo(product, options => options.Excluding(x => x.Signer));
        }

        [Fact]
        public async Task CreateProtocol_CreatesProtocol()
        {
            var protocol = new Protocol
            {
                Name = Any<string>(),
                CreationDate = Any<string>(),
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProtocolRepository(context, _mapper);

            var createdProtocol = await repository.CreateProtocol(protocol);

            createdProtocol.Name.Should().Be(protocol.Name);
        }

        [Fact]
        public async Task UpdateProtocol_UpdatesProtocol()
        {
            var protocol = new Protocol
            {
                Id= 1,
                Name = Any<string>(),
                ReportName = Any<string>(),
                EnvelopeId = Any<string>(),
                IsSigned = true,
                SignerId = 1,
                IsCompleted = true,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProtocolRepository(context, _mapper);

            var updatedProtocol = await repository.UpdateProtocol(protocol);

            updatedProtocol.Name.Should().Be(protocol.Name);
            updatedProtocol.ReportName.Should().Be(protocol.ReportName);
            updatedProtocol.EnvelopeId.Should().Be(protocol.EnvelopeId);
            updatedProtocol.IsSigned.Should().Be(protocol.IsSigned);
            updatedProtocol.SignerId.Should().Be(protocol.SignerId);
            updatedProtocol.IsCompleted.Should().Be(protocol.IsCompleted);
        }

        [Fact]
        public async Task DeleteProtocol_DeletesProtocol()
        {
            // Arrange
            var protocol = new ProtocolEntity
            {
                Name = Any<string>(),
                CreationDate = Any<string>(),
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProtocolRepository(context, _mapper);

            context.Protocols.Add(protocol);
            var createdEntity = context.SaveChangesAsync();

            // Act
            await repository.DeleteProtocol(protocol.Id);

            // Assert
            var foundEntity = await context.Protocols.FirstOrDefaultAsync(x => x.Id == protocol.Id);
            foundEntity.Should().BeNull();
        }
    }
}
