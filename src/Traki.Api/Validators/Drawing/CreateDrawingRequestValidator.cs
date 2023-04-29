using FluentValidation;
using Traki.Api.Contracts.Drawing;

namespace Traki.Api.Validators.Drawing
{
    public class CreateDrawingRequestValidator: AbstractValidator<CreateDrawingRequest>
    {
        public CreateDrawingRequestValidator() 
        {
            RuleFor(p => p.Drawing).SetValidator(new DrawingValidator());
        }
    }
}
