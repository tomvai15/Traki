using FluentValidation;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Validators.Section
{
    public class ChecklistValidator: AbstractValidator<ChecklistDto>
    {
        public ChecklistValidator() 
        {
            RuleForEach(x => x.Items).SetValidator(new ItemValidator());
        }
    }
}
