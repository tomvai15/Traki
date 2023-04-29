using FluentValidation;
using Traki.Api.Contracts.Product;

namespace Traki.Api.Validators.Product
{
    public class UpdateProductRequestValidator: AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator() 
        {
            RuleFor(x => x.Product).SetValidator(new ProductValidator());
        }
    }
}
