using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Product;
using Traki.Api.Contracts.Protocol;
using Traki.Domain.Constants;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/projects/{projectId}/products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IProductHandler _productHandler;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IMapper _mapper;

        public ProductsController(IClaimsProvider claimsProvider, IProductsRepository productsRepository, IProductHandler productHandler, IMapper mapper)
        {
            _claimsProvider = claimsProvider;
            _productsRepository = productsRepository;
            _productHandler = productHandler;
            _mapper = mapper;
        }

        [HttpGet("{productId}")]
        [Authorize]
        public async Task<ActionResult<GetProductResponse>> GetProduct(int projectId, int productId)
        {
            var product = await _productsRepository.GetProduct(productId);

            if (product == null)
            {
                return NotFound();
            }
            var response = new GetProductResponse
            {
                Product = _mapper.Map<ProductDto>(product)
            };

            return Ok(response);
        }

        [HttpPut("{productId}")]
        [Authorize]
        [Authorize(Policy = AuthPolicy.ProductIdInRouteValidation)]
        public async Task<ActionResult> UpdateProduct(int projectId, int productId, [FromBody]UpdateProductRequest updateProductRequest)
        {
            var product = _mapper.Map<Product>(updateProductRequest.Product);

            var productFromDb = await _productsRepository.GetProduct(productId);

            productFromDb.Name = product.Name;

            await _productsRepository.UpdateProduct(product);

            return Ok();
        }

        [HttpDelete("{productId}")]
        [Authorize]
        [Authorize(Policy = AuthPolicy.ProductIdInRouteValidation)]
        public async Task<ActionResult> DeleteProduct(int projectId, int productId)
        {
            await _productsRepository.DeleteProduct(productId);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetProductsResponse>> GetProducts(int projectId)
        {
            var products = await _productsRepository.GetProducts(projectId);

            return _mapper.Map<GetProductsResponse>(products);
        }

        [HttpPost("{productId}/protocols")]
        [Authorize]
        public async Task<ActionResult> AddProtocol(int productId, AddProtocolRequest addProtocolRequest)
        {
            await _productHandler.AddProtocolToProduct(productId, addProtocolRequest.ProtocolId);
            return Ok();
        }

        [HttpPost("{productId}/status")]
        [Authorize]
        public async Task<ActionResult> UpdateProductStatus(int productId)
        {
            var product = await _productsRepository.GetProduct(productId);

            product.Status = ProductStatus.Completed;
            await _productsRepository.UpdateProduct(product);

            return Ok();
        }

        [HttpGet("{productId}/protocols")]
        [Authorize]
        public async Task<ActionResult<GetProductProtocolsResponse>> GetProtocols(int productId)
        {
            var protocols = await _productHandler.GetProtocols(productId);

            var response = new GetProductProtocolsResponse
            {
                Protocols = _mapper.Map<IEnumerable<ProtocolDto>>(protocols)
            };
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GetProductResponse>> PostProduct(int projectId, CreateProductRequest createProjectRequest)
        {
            var product = _mapper.Map<Product>(createProjectRequest);

            _claimsProvider.TryGetUserId(out var userId);
            product.Status = "Active";
            product.AuthorId = userId;
            var createdProduct = await _productsRepository.CreateProduct(product);

            var response = new GetProductResponse
            {
                Product = _mapper.Map<ProductDto>(createdProduct)
            };

            return Ok(response);
        }
    }
}
