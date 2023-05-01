using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Drawing;

namespace Traki.Api.Validators.Drawing
{
    [ExcludeFromCodeCoverage]
    public class DrawingValidator: AbstractValidator<DrawingDto>
    {
        public DrawingValidator()
        {
            RuleFor(p => p.Title).NotEmpty().MaximumLength(50).NoSpecialSymbols();
            RuleFor(p => p.ImageName).ValidFileName();
        }
    }
}
