using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traki.Domain.Models
{
    public class Protocol
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ReportName { get; set; }
        public string? EnvelopeId { get; set; }
        public bool IsTemplate { get; set; }
        public int ProductId { get; set; }
    }
}
