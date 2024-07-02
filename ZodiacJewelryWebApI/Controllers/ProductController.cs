using Application.IService;
using Application.ServiceResponse;
using Application.Services;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZodiacJewelryWebApI.Controllers;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("Allow")]
    [Route("api/products")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IZodiacProductService _zodiacService;


        public ProductController(IProductService productService, IZodiacProductService zodiacService)
        {
            _productService = productService;
            _zodiacService = zodiacService;
        }

        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync([FromQuery] int page = 1,  [FromQuery] int pageSize = 5, [FromQuery] string search = "", [FromQuery] string sort = "")
        {
            var result = await _productService.GetAllProductsAsync(page, pageSize, search, sort);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "Staff,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(CreateProductDTO product, int zodiacId)
        {
            var result = await _productService.CreateProductAsync(product, zodiacId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "Staff,Admin")]
        [HttpPut("{id}/zodiac/{zodiacId}")]
        public async Task<IActionResult> UpdateProductAsync(int id, CreateProductDTO product, int zodiacId)
        {
         

            var result = await _productService.UpdateProductAsync(product, zodiacId);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "Staff,Admin")]
        [HttpDelete("{id}")]
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
