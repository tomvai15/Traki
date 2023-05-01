using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Validators.Section
{
    [ExcludeFromCodeCoverage]
    public class ChecklistValidator: AbstractValidator<ChecklistDto>
    {
        public ChecklistValidator() 
        {
            RuleForEach(x => x.Items).SetValidator(new ItemValidator());
        }
    }
}
