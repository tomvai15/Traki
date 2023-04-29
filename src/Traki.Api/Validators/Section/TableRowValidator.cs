using FluentValidation;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Validators.Section
{
    public class TableRowValidator: AbstractValidator<TableRowDto>
    {
        public TableRowValidator() 
        {
            RuleForEach(x => x.RowColumns).SetValidator(new RowCollumnValidator());
        }
    }
}
