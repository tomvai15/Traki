using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Product;

namespace Traki.Api.Validators.Product
{
    [ExcludeFromCodeCoverage]
    public class AddProtocolRequestValidator: AbstractValidator<AddProtocolRequest>
    {
        public AddProtocolRequestValidator()
        {
          //  RuleFor(x => x.Product).SetValidator(new ProductValidator());
        }
    }
}
