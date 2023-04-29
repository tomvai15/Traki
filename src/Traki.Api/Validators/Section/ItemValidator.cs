using FluentValidation;
using Traki.Api.Contracts.Section.Items;

namespace Traki.Api.Validators.Section
{
    public class ItemValidator: AbstractValidator<ItemDto>
    {
        public ItemValidator() 
        { 
            RuleFor(x=> x.Name).NotEmpty().NoSpecialSymbols();
        }
    }
}
