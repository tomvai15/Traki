using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models.Drawing;

namespace Traki.Domain.Models
{
    public class Recommendation
    {
        public IEnumerable<DefectRecomendation> Defects { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
