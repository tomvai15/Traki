using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Product;
using Traki.Api.Data.Repositories;
using Traki.Api.Models;

namespace Traki.Api.Controllers
{
    [Route("api/projects/{projectId}/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductsRepository productsRepository, IMapper mapper)
        {
            _productsRepository = productsRepository;
            _mapper = mapper;
        }

        [HttpGet(("{productId}"))]
        public async Task<ActionResult<GetProductResponse>> GetProduct(int projectId, int productId)
        {
            var product = await _productsRepository.GetProduct(productId);

            if (product == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetProductResponse>(product);
        }

        [HttpGet]
        public async Task<ActionResult<GetProductsResponse>> GetProducts(int projectId)
        {
            var products = await _productsRepository.GetProducts(projectId);

            return _mapper.Map<GetProductsResponse>(products);
        }

        [HttpPost]
        public async Task<ActionResult<GetProductResponse>> PostProduct(int projectId, CreateProductRequest createProjectRequest)
        {
            var product = _mapper.Map<Product>(createProjectRequest);

            var createdProduct = await _productsRepository.CreateProduct(product);

            return CreatedAtAction("GetProduct", new { productId = createdProduct.Id }, createdProduct);
        }
    }
}
