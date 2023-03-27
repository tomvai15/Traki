using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Product;
using Traki.Api.Contracts.Protocol;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/projects/{projectId}/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IProductHandler _productHandler;
        private readonly IMapper _mapper;

        public ProductsController(IProductsRepository productsRepository, IProductHandler productHandler, IMapper mapper)
        {
            _productsRepository = productsRepository;
            _productHandler = productHandler;
            _mapper = mapper;
        }

        [HttpGet("{productId}")]
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

        [HttpPost("{productId}/protocols")]
        public async Task<ActionResult> AddProtocol(int productId, AddProtocolRequest addProtocolRequest)
        {
            await _productHandler.AddProtocolToProduct(productId, addProtocolRequest.ProtocolId);
            return Ok();
        }

        [HttpGet("{productId}/protocols")]
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
        public async Task<ActionResult<GetProductResponse>> PostProduct(int projectId, CreateProductRequest createProjectRequest)
        {
            var product = _mapper.Map<Product>(createProjectRequest);

            var createdProduct = await _productsRepository.CreateProduct(product);

            return CreatedAtAction("GetProduct", new { productId = createdProduct.Id }, createdProduct);
        }
    }
}
