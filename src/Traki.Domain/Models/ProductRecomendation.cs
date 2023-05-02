using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models.Drawing;

namespace Traki.Domain.Models
{
    public class ProductRecomendation
    {
        public Product Product { get; set; }
        public int DefectCount { get; set; }
        public int ProtocolsCount { get; set; }
    }
}
