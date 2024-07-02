using Application.IService;
using Application.ViewModels.ProductDTO;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet("products/zodiac")]
        public async Task<IActionResult> GetAllZodiacProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = await _zodiacService.GetAllZodiacProduct(page, pageSize);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "Staff,Admin,Customer")]
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

        [Authorize(Roles = "Staff,Admin")]
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

        [Authorize(Roles = "Staff,Admin")]
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


        [Authorize(Roles = "Staff,Admin")]
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

        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet("zodiac/{zodiacId}/products")]
        public async Task<IActionResult> GetAllProductsByZodiacId(int zodiacId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromQuery] string search ="", [FromQuery] string sort = "")
        {
            var result = await _zodiacService.GetAllProductsByZodiacId(zodiacId, page, pageSize, search, sort);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
