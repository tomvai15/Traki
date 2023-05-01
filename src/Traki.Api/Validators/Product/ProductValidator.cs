using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Product;
using Traki.Domain.Constants;

namespace Traki.Api.Validators.Product
{
    [ExcludeFromCodeCoverage]
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NoSpecialSymbols();
            RuleFor(p => p.Status).Must(x => x == ProductStatus.Active || x == ProductStatus.Completed)
                .WithMessage("Product status must be Active or Completed");

        }
    }
}
