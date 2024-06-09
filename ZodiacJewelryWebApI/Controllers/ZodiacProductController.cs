﻿using Application.IService;
using Application.ViewModels.ProductDTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("Allow")]
    [Route("api/zodiacproducts")]
    [ApiController]
    public class ZodiacProductController : BaseController
    {
        private readonly IZodiacProductService _zodiacService;

        public ZodiacProductController(IZodiacProductService zodiacService)
        {
            _zodiacService = zodiacService;
        }

        [HttpGet("all-zodiacProduct")]
        public async Task<IActionResult> GetAllZodiacProducts()
        {
            var result = await _zodiacService.GetAllZodiacProduct();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("zodiacProduct-by-id/{id}")]
        public async Task<IActionResult> GetZodiacProductById(int id)
        {
            var result = await _zodiacService.GetZodiacProductById(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("zodiacProduct-new")]
        public async Task<IActionResult> AddZodiacProduct([FromBody] ZodiacProductDTO zodiacProductDTO)
        {
            var result = await _zodiacService.AddZodiacProduct(zodiacProductDTO);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("zodiacProduct-update")]
        public async Task<IActionResult> UpdateZodiacProduct( [FromBody] ZodiacProductDTO zodiacProductDTO)
        {
          

            var result = await _zodiacService.UpdateZodiacProduct(zodiacProductDTO);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete("order-remove/{id}")]
        public async Task<IActionResult> DeleteZodiacProduct(int id)
        {
            var result = await _zodiacService.DeleteZodiacProduct(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("zodiac/{zodiacId}")]
        public async Task<IActionResult> GetAllProductsByZodiacId(int zodiacId)
        {
            var result = await _zodiacService.GetAllProductsByZodiacId(zodiacId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
