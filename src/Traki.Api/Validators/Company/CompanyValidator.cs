using FluentValidation;
using Traki.Api.Contracts.Company;

namespace Traki.Api.Validators.Company
{
    public class CompanyValidator : AbstractValidator<CompanyDto>
    {
        public CompanyValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NoSpecialSymbols();
            RuleFor(p => p.Address).NotEmpty().NoSpecialSymbols();
            RuleFor(p => p.ImageName).ValidFileName();
        }
    }
}
