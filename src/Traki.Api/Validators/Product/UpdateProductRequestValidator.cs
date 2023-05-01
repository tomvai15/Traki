using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Product;

namespace Traki.Api.Validators.Product
{
    [ExcludeFromCodeCoverage]
    public class UpdateProductRequestValidator: AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator() 
        {
            RuleFor(x => x.Product).SetValidator(new ProductValidator());
        }
    }
}
