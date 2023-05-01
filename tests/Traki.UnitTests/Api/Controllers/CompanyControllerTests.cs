using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Traki.Api.Contracts.Company;
using Traki.Api.Controllers;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Api.Controllers
{
    public class CompanyControllerTests
    {
        private readonly Mock<ICompaniesRepository> _mockCompaniesRepository;
        private readonly IMapper _mapper;
        private readonly CompanyController _controller;

        public CompanyControllerTests()
        {
            _mockCompaniesRepository = new Mock<ICompaniesRepository>();
            _mapper = CreateMapper();
            _controller = new CompanyController(_mockCompaniesRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetCompany_ReturnsOkObjectResult_WhenCompanyExists()
        {
            // Arrange
            var companyId = 1;
            var company = new Company { Id = companyId, Name = "Test Company" };
            var response = new GetCompanyResponse
            {
                Company = new CompanyDto
                {
                    Id = companyId,
                    Name = "Test Company"
                }
            };

            _mockCompaniesRepository.Setup(repo => repo.GetCompany(companyId)).ReturnsAsync(company);

            // Act
            var result = await _controller.GetCompany(companyId);

            // Assert
            var data = result.ShouldBeOfType<GetCompanyResponse>();
            data.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task UpdateCompany_ReturnsBadRequest_WhenCompanyIdAndRequestCompanyIdAreDifferent()
        {
            // Arrange
            var companyId = 1;
            var updateRequest = new UpdateCompanyRequest { 
                Company = new CompanyDto { Id = 2, Name = "Updated Test Company" } 
            };

            // Act
            var result = await _controller.UpdateCompanyCompany(companyId, updateRequest);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task UpdateCompany_ReturnsOkResult_WhenCompanyIsUpdated()
        {
            // Arrange
            var companyId = 1;
            var company = new Company { Id = companyId, Name = "Test Company" };
            var updateRequest = new UpdateCompanyRequest { 
                Company = new CompanyDto { Id = companyId, Name = "Updated Test Company" } 
            };

            // Act
            var result = await _controller.UpdateCompanyCompany(companyId, updateRequest);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}
