using FluentValidation;
using Traki.Api.Contracts.Product;
using Traki.Domain.Constants;

namespace Traki.Api.Validators.Product
{
    public class CreateProductRequestValidator: AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator() 
        {
            RuleFor(x => x.Product).SetValidator(new ProductValidator());
        }
    }
}
