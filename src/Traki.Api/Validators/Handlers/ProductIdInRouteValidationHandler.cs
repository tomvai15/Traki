using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Traki.Api.Validators.Requirements;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;

namespace Traki.Api.Validators.Handlers
{
    public class ProductIdInRouteValidationHandler : AuthorizationHandler<ProductIdInRouteValidation>, IAuthorizationHandler
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IActionContextAccessor _actionContextAccessor;
        public ProductIdInRouteValidationHandler(IProductsRepository productsRepository, IClaimsProvider claimsProvider, IActionContextAccessor actionContextAccessor)
        {
            _productsRepository = productsRepository;
            _claimsProvider = claimsProvider;
            _actionContextAccessor = actionContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductIdInRouteValidation requirement)
        {
            if (!_claimsProvider.TryGetUserId(out int userId))
            {
                return;
            }

            if (context.Resource is HttpContext httpContext)
            {
                int productId = int.Parse(httpContext.GetRouteValue("productId").ToString());
                var product = await _productsRepository.GetProduct(productId);
                if (product == null)
                {
                    return;
                }

                if (product.AuthorId != userId)
                {
                    context.Fail();
                    return;
                }
            }

            context.Succeed(requirement);
            return;
        }
    }
}
