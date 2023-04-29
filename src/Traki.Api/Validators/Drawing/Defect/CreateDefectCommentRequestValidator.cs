using FluentValidation;
using Traki.Api.Contracts.Drawing.Defect;

namespace Traki.Api.Validators.Drawing.Defect
{
    public class CreateDefectCommentRequestValidator: AbstractValidator<CreateDefectCommentRequest>
    {
        public CreateDefectCommentRequestValidator() 
        {
            RuleFor(x => x.DefectComment).SetValidator(new DefectCommentValidator());
        }
    }
}
