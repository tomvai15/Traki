using FluentValidation;
using Traki.Api.Contracts.Company;
using Traki.Api.Validators.Project;

namespace Traki.Api.Validators.Company
{
    public class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
    {
        public UpdateCompanyRequestValidator()
        {
            RuleFor(p => p.Company).SetValidator(new CompanyValidator());
        }
    }
}
