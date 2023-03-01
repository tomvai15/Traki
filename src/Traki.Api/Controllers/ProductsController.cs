using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Product;
using Traki.Api.Contracts.Project;
using Traki.Api.Handlers;
using Traki.Api.Models;

namespace Traki.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsHandler _productsHandler;
        private readonly IMapper _mapper;

        public ProductsController(IProductsHandler productsHandler, IMapper mapper)
        {
            _productsHandler = productsHandler;
            _mapper = mapper;
        }

        [HttpGet(("{productId}"))]
        public async Task<ActionResult<GetProductResponse>> GetProduct(int productId)
        {
            var product = await _productsHandler.GetProduct(productId);

            if (product == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetProductResponse>(product);
        }

        [HttpGet]
        public async Task<ActionResult<GetProductsResponse>> GetProducts()
        {
            var products = await _productsHandler.GetProducts();

            return _mapper.Map<GetProductsResponse>(products);
        }

        [HttpPost]
        public async Task<ActionResult<GetProductResponse>> PostProduct(CreateProductRequest createProjectRequest)
        {
            var product = _mapper.Map<Product>(createProjectRequest);

            var createdProduct = await _productsHandler.CreateProduct(product);

            return CreatedAtAction("GetProduct", new { productId = createdProduct.Id }, createdProduct);
        }
    }
}
