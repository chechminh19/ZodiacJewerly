using Application.IService;
using Application.Services;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Application.ViewModels.ZodiacDTO;
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


        [HttpGet]
        public async Task<IActionResult> GetAllZodiacs(int page)
        {
            var result = await _zodiacService.GetAllZodiacs(page);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetZodiacById(int id)
        {
            var result = await _zodiacService.GetZodiacById(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateZodiac([FromBody] ZodiacUpdateDTO zodiac)
        {


            var result = await _zodiacService.UpdateZodiac(zodiac);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
