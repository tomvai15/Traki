using FluentValidation;
using Traki.Api.Contracts.Product;

namespace Traki.Api.Validators.Product
{
    public class AddProtocolRequestValidator: AbstractValidator<AddProtocolRequest>
    {
        public AddProtocolRequestValidator()
        {
          //  RuleFor(x => x.Product).SetValidator(new ProductValidator());
        }
    }
}
