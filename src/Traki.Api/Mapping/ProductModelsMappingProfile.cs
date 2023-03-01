using AutoMapper;
using Traki.Api.Contracts.Product;
using Traki.Api.Models;

namespace Traki.Api.Mapping
{
    public class ProductModelsMappingProfile: Profile
    {
        public ProductModelsMappingProfile()
        {
            CreateMap<Product, GetProductResponse>()
                .ForMember(p => p.Product, opt => opt.MapFrom(p => p));

            CreateMap<IEnumerable<Product>, GetProductsResponse>()
                .ForMember(p => p.Products, opt => opt.MapFrom(p => p));

            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<CreateProductRequest, Product>()
                .IncludeMembers(p => p.Product);
        }
    }
}
