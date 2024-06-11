using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ProductImageDTO
{
    public class ProductImageDTO
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductId { get; set; }
    }
}
