using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Section.Items;

namespace Traki.Api.Validators.Section
{
    [ExcludeFromCodeCoverage]
    public class ItemValidator: AbstractValidator<ItemDto>
    {
        public ItemValidator() 
        { 
            RuleFor(x=> x.Name).NotEmpty().NoSpecialSymbols();
        }
    }
}
