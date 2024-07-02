using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.OrderDTO
{
    public class ProductToCreateOrderDTO
    {
        public int ProductId { get; set; }
        public string ZodiacName { get; set; }
        public string NameProduct { get; set; }
        public string DescriptionProduct { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        //public int CategoryId { get; set; }
        public string NameCategory { get; set; }
        //public int MaterialId { get; set; }
        public string NameMaterial { get; set; }
        //public int GenderId { get; set; }
        public string NameGender { get; set; }
        public string ImageUrl { get; set; }
        public int OrderId { get; set; }
    }
}
