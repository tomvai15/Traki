using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Drawing.Defect;

namespace Traki.Api.Validators.Drawing.Defect
{
    [ExcludeFromCodeCoverage]
    public class CreateDefectRequestValidator: AbstractValidator<CreateDefectRequest>
    {
        public CreateDefectRequestValidator()
        {
            RuleFor(x => x.Defect).SetValidator(new DefectValidator());
        }
    }
}
