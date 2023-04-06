using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models.Drawing;

namespace Traki.Domain.Models
{
    public class DefectRecomendation
    {
        public Defect Defect { get; set; }
        public int ProjectId { get; set; }
        public int ProductId { get; set; }
    }
}
