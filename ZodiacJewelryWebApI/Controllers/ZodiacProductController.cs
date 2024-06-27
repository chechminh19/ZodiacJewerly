using Application.IService;
using Application.ViewModels.ProductDTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("Allow")]
    [Route("api")]
    [ApiController]
    public class ZodiacProductController : BaseController
    {
        private readonly IZodiacProductService _zodiacService;

        public ZodiacProductController(IZodiacProductService zodiacService)
        {
            _zodiacService = zodiacService;
        }

        [HttpGet("products/zodiac")]
        public async Task<IActionResult> GetAllZodiacProducts([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = await _zodiacService.GetAllZodiacProduct(page, pageSize);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("products/zodiac/{id}")]
        public async Task<IActionResult> GetZodiacProductById(int id)
        {
            var result = await _zodiacService.GetZodiacProductById(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("products/zodiac")]
        public async Task<IActionResult> AddZodiacProduct([FromBody] ZodiacProductDTO zodiacProductDTO)
        {
            var result = await _zodiacService.AddZodiacProduct(zodiacProductDTO);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("products/zodiac")]
        public async Task<IActionResult> UpdateZodiacProduct( [FromBody] ZodiacProductDTO zodiacProductDTO)
        {
          

            var result = await _zodiacService.UpdateZodiacProduct(zodiacProductDTO);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete("products/zodiac/{id}")]
        public async Task<IActionResult> DeleteZodiacProduct(int id)
        {
            var result = await _zodiacService.DeleteZodiacProduct(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("zodiac/{zodiacId}/products")]
        public async Task<IActionResult> GetAllProductsByZodiacId(int zodiacId, [FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string search, [FromQuery] Dictionary<string, string> filters, [FromQuery] string sort = "id")
        {
            var result = await _zodiacService.GetAllProductsByZodiacId(zodiacId, page, pageSize, search, filters, sort);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
