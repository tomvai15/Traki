﻿using Traki.Api.Contracts.Product;

namespace Traki.Api.Contracts.Recommendation
{
    public class RecommendationDto
    {
        public IEnumerable<DefectRecomendationDto> Defects { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
