using FluentValidation;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Validators.Section
{
    public class TableValidator: AbstractValidator<TableDto>
    {
        public TableValidator() 
        {
            RuleForEach(x => x.TableRows).SetValidator(new TableRowValidator());
        }

    }
}
