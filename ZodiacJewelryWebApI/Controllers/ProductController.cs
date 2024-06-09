using Application.IService;
using Application.Services;
using Application.ViewModels.ProductDTO;
using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZodiacJewelryWebApI.Controllers;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("Allow")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IZodiacProductService _zodiacService;


        public ProductController(IProductService productService, IZodiacProductService zodiacService)
        {
            _productService = productService;
            _zodiacService = zodiacService;
        }

        [HttpGet("all-products")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var result = await _productService.GetAllProductsAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("product-by-id/{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpPost("product-new")]
        public async Task<IActionResult> CreateProductAsync(CreateProductDTO product, int zodiacId)
        {
            var result = await _productService.CreateProductAsync(product, zodiacId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPut("product-update")]
        public async Task<IActionResult> UpdateProductAsync(int id, CreateProductDTO product, int zodiacId)
        {
         

            var result = await _productService.UpdateProductAsync(product, zodiacId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete("product-remove/{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
