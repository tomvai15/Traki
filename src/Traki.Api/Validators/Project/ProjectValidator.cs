using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Project;

namespace Traki.Api.Validators.Project
{
    [ExcludeFromCodeCoverage]
    public class ProjectValidator : AbstractValidator<ProjectDto>
    {
        public ProjectValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NoSpecialSymbols();
            RuleFor(p => p.ClientName).NotEmpty().NoSpecialSymbols();
            RuleFor(p => p.Address).NotEmpty().NoSpecialSymbols();
            RuleFor(p => p.ImageName).ValidFileName();
        }
    }
}