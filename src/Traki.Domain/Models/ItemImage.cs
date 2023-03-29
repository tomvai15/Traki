using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traki.Domain.Models
{
    public class ItemImage
    {
        public string ItemId { get; set; }
        public string AttachmentName { get; set; }
        public string ImageBase64 { get; set; }
    }
}
