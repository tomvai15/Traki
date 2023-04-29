using FluentValidation;
using Traki.Api.Contracts.Drawing.Defect;

namespace Traki.Api.Validators.Drawing.Defect
{
    public class CreateDefectRequestValidator: AbstractValidator<CreateDefectRequest>
    {
        public CreateDefectRequestValidator()
        {
            RuleFor(x => x.Defect).SetValidator(new DefectValidator());
        }
    }
}
