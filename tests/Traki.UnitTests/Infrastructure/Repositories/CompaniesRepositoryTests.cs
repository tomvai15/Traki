using AutoMapper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class CompaniesRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public CompaniesRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task GetCompany_ReturnsCompany()
        {
            int companyId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new CompaniesRepository(context, _mapper);

            var result = await repository.GetCompany(companyId);

            result.Id.Should().Be(companyId);
        }

        [Fact]
        public async Task UpdateCompany_UpdatesCompany()
        {
            var company = new Company
            {
                Id = 1,
                Name = "Test",
                Address = "Test",
                ImageName = "Test"
            };


            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new CompaniesRepository(context, _mapper);

            await repository.UpdateCompany(company.Id, company);


            var companyEntity = context.Companies.FirstOrDefault(x => x.Id == company.Id);

            companyEntity.Name.Should().Be(company.Name);
            companyEntity.ImageName.Should().Be(company.ImageName);
            companyEntity.Address.Should().Be(company.Address);
        }
    }
}
