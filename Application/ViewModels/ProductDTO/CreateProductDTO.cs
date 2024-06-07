using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ProductDTO
{
    public class CreateProductDTO
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name can't be longer than 100 characters")]
        public string NameProduct { get; set; }

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public string DescriptionProduct { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [ValidateType(typeof(double), ErrorMessage = "Price must be a valid number")]
        [NonNegative(ErrorMessage = "Price must be a non-negative number")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [NonNegative(ErrorMessage = "Quantity must be a non-negative integer")]
        [ValidateType(typeof(int), ErrorMessage = "Quantity must be a valid number")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [ValidateType(typeof(int), ErrorMessage = "Category ID must be an integer")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Material ID is required")]
        [ValidateType(typeof(int), ErrorMessage = "Material ID must be an integer")]
        public int MaterialId { get; set; }

        [Required(ErrorMessage = "Gender ID is required")]
        [ValidateType(typeof(int), ErrorMessage = "Gender ID must be an integer")]
        public int GenderId { get; set; }

        





    }

    

}
