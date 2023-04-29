using FluentValidation;
using Traki.Api.Contracts.Project;

namespace Traki.Api.Validators.Project
{
    public class ProjectValidator : AbstractValidator<ProjectDto>
    {
        public ProjectValidator()
        {
            RuleFor(p => p.Name).NotEmpty().Matches("^[a-zA-ZąčęėįšųūžĄČĘĖĮŠŲŪŽ0-9\\s.,:;?!%()_&+={}\\[\\]|\\\\/~$^+-]*$").WithMessage("Specials symbols are not allowed");
            RuleFor(p => p.ClientName).NotEmpty();
            RuleFor(p => p.Address).NotEmpty();
        }
    }
}