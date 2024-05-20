using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductImage
    {
        public string? ImageUrl { get; set; }
        public string? PublicId { get; set; }      
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
