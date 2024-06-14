﻿using Application.IService;
using Application.Services;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
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


        [HttpGet]
        public async Task<IActionResult> GetAllOrderGetAllOrderGetAllImageInfors()
        {
            var result = await _imageService.GetAllOrderGetAllImageInfors();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

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
