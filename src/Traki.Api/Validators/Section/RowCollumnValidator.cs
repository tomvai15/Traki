using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Validators.Section
{
    [ExcludeFromCodeCoverage]
    public class RowCollumnValidator: AbstractValidator<RowColumnDto>
    {
        public RowCollumnValidator() 
        {
            RuleFor(x => x.Value).NoSpecialSymbols();
        }
    }
}
