using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Validators.Section
{
    [ExcludeFromCodeCoverage]
    public class SectionValidator: AbstractValidator<SectionDto>
    {
        public SectionValidator() 
        {
            RuleFor(p => p.Name).NotEmpty().NoSpecialSymbols();
            RuleFor(p => p.Checklist).SetValidator(new ChecklistValidator()).When(x=> x.Checklist != null);
            RuleFor(p => p.Table).SetValidator(new TableValidator()).When(x => x.Table != null);
        }
    }
}
