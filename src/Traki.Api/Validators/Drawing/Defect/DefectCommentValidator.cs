using FluentValidation;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Domain.Models.Drawing;

namespace Traki.Api.Validators.Drawing.Defect
{
    public class DefectCommentValidator: AbstractValidator<DefectCommentDto>
    {
        public DefectCommentValidator() 
        {
            RuleFor(x => x.Text).NoSpecialSymbols().MaximumLength(250);
        }
    }
}
