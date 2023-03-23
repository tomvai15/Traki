using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Extensions;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;

namespace Traki.Infrastructure.Repositories
{
    public class CompaniesRepository : ICompaniesRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public CompaniesRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Company> GetCompany(int companyId)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(p => p.Id == companyId);

            company.RequiresToBeNotNullEnity();

            return _mapper.Map<Company>(company);
        }

        public async Task UpdateCompany(int companyId, Company company)
        {
            var companyEntity = await _context.Companies.FirstOrDefaultAsync(p => p.Id == companyId);
            company.RequiresToBeNotNullEnity();

            companyEntity.Name = company.Name;
            companyEntity.Address = company.Address;
            companyEntity.ModifiedDate = DateTime.UtcNow;
            companyEntity.ImageName = company.ImageName;

            await _context.SaveChangesAsync();
        }
    }
}
