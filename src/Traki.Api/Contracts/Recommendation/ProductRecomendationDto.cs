using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Api.Contracts.Product;
using Traki.Domain.Models.Drawing;

namespace Traki.Api.Contracts.Recommendation
{
    public class ProductRecomendationDto
    {
        public ProductDto Product { get; set; }
        public int DefectCount { get; set; }
        public int ProtocolsCount { get; set; }
    }
}
