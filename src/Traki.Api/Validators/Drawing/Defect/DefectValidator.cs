using FluentValidation;
using Traki.Api.Contracts.Drawing.Defect;

namespace Traki.Api.Validators.Drawing.Defect
{
    public class DefectValidator: AbstractValidator<DefectDto>
    {
        public DefectValidator() 
        {
            RuleFor(x => x.Title).NoSpecialSymbols();
            RuleFor(x => x.Description).NoSpecialSymbols();
        }
    }
}
