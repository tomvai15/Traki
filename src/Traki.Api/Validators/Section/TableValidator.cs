using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Validators.Section
{
    [ExcludeFromCodeCoverage]
    public class TableValidator: AbstractValidator<TableDto>
    {
        public TableValidator() 
        {
            RuleForEach(x => x.TableRows).SetValidator(new TableRowValidator());
        }

    }
}
