using Application.IService;
using Application.Services;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("Allow")]
    [Route("api/image")]
    [ApiController]
    public class ImageController : BaseController
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet]
        public async Task<IActionResult> AllImageInfors([FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromQuery] string search = "", [FromQuery] string sort = "")
        {
            var result = await _imageService.GetAllImageInfors(page, pageSize, search, sort);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageInforById(int id)
        {
            var result = await _imageService.GetImageInforById(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
  
    }
}
