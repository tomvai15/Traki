using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Project;

namespace Traki.Api.Validators.Project
{
    [ExcludeFromCodeCoverage]
    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidator() 
        {
            RuleFor(p => p.Project).SetValidator(new ProjectValidator());
        }
    }
}
