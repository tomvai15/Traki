using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface ICompaniesRepository
    {
        Task<Company> GetCompany(int id);
        Task UpdateCompany(int id, Company company);
    }
}
