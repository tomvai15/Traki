using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Drawing;

namespace Traki.Api.Validators.Drawing
{
    [ExcludeFromCodeCoverage]
    public class CreateDrawingRequestValidator: AbstractValidator<CreateDrawingRequest>
    {
        public CreateDrawingRequestValidator() 
        {
            RuleFor(p => p.Drawing).SetValidator(new DrawingValidator());
        }
    }
}
