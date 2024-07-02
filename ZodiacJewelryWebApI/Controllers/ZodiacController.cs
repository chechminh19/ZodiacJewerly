using Application.IService;
using Application.Services;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Application.ViewModels.ZodiacDTO;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAllZodiacs([FromQuery] int page = 1 ,[FromQuery] int pageSize = 5, [FromQuery] string search = "",  [FromQuery] string sort = "id")
        {
            var result = await _zodiacService.GetAllZodiacs(page, pageSize, search, sort);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "Staff,Admin,Customer")]
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
        [Authorize(Roles = "Staff,Admin")]
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
