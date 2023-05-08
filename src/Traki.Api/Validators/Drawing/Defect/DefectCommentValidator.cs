using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Drawing.Defect;

namespace Traki.Api.Validators.Drawing.Defect
{
    [ExcludeFromCodeCoverage]
    public class DefectCommentValidator: AbstractValidator<DefectCommentDto>
    {
        public DefectCommentValidator() 
        {
            RuleFor(x => x.Text).NoSpecialSymbols().MaximumLength(250);
        }
    }
}
