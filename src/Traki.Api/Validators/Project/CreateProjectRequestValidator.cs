using FluentValidation;
using Traki.Api.Contracts.Project;

namespace Traki.Api.Validators.Project
{
    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidator() 
        {
            RuleFor(p => p.Project).SetValidator(new ProjectValidator());
        }
    }
}
