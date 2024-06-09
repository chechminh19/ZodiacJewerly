using Application.IService;
using Application.Services;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("Allow")]
    [Route("api/zodiacs")]
    [ApiController]
    public class ZodiacController : BaseController
    {
        private readonly IZodiacService _zodiacService;

        public ZodiacController(IZodiacService zodiacService)
        {
            _zodiacService = zodiacService;
        }


        [HttpGet("all-zodiacs")]
        public async Task<IActionResult> GetAllZodiacs()
        {
            var result = await _zodiacService.GetAllZodiacs();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("zodiac-by-id/{id}")]
        public async Task<IActionResult> GetZodiacById(int id)
        {
            var result = await _zodiacService.GetZodiacById(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
