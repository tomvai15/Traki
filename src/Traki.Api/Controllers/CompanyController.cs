using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Company;
using Traki.Domain.Models;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController: ControllerBase
    {
        private readonly ICompaniesRepository _companiesRepository;
        private readonly IMapper _mapper;

        public CompanyController(ICompaniesRepository companiesRepository, IMapper mapper)
        {
            _companiesRepository = companiesRepository;
            _mapper = mapper;
        }

        [HttpGet(("{companyId}"))]
        public async Task<ActionResult<GetCompanyResponse>> GetCompany(int companyId)
        {
            var project = await _companiesRepository.GetCompany(companyId);

            return _mapper.Map<GetCompanyResponse>(project);
        }

        [HttpPatch(("{companyId}"))]
        public async Task<ActionResult> UpdateCompanyCompany(int companyId, [FromBody] UpdateCompanyRequest updateCompanyRequest)
        {
            if (companyId != updateCompanyRequest.Company.Id)
            {
                return BadRequest();
            }

            var company = _mapper.Map<Company>(updateCompanyRequest.Company);
            await _companiesRepository.UpdateCompany(companyId, company);

            return Ok();
        }
    }
}
