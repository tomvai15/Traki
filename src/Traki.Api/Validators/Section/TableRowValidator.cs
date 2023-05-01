using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Validators.Section
{
    [ExcludeFromCodeCoverage]
    public class TableRowValidator: AbstractValidator<TableRowDto>
    {
        public TableRowValidator() 
        {
            RuleForEach(x => x.RowColumns).SetValidator(new RowCollumnValidator());
        }
    }
}
