using FluentValidation;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Validators.Section
{
    public class RowCollumnValidator: AbstractValidator<RowColumnDto>
    {
        public RowCollumnValidator() 
        {
            RuleFor(x => x.Value).NoSpecialSymbols();
        }
    }
}
