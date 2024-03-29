﻿using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using Traki.Api.Contracts.Product;

namespace Traki.Api.Validators.Product
{
    [ExcludeFromCodeCoverage]
    public class CreateProductRequestValidator: AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator() 
        {
            RuleFor(x => x.Product).SetValidator(new ProductValidator());
        }
    }
}
