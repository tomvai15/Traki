using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Drawing.Defect;

namespace Traki.Api.Validators.Drawing.Defect
{
    [ExcludeFromCodeCoverage]
    public class CreateStatusChangeRequestValidator: AbstractValidator<CreateStatusChangeRequest>
    {
        public CreateStatusChangeRequestValidator() 
        {
            RuleFor(x => x.StatusChange).SetValidator(new StatusChangeValidator());
        }
    }
}
