using Application.IService;
using Application.ServiceResponse;
using Application.ViewModels.ProductDTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("http://localhost:4200")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<ProductDTO>>>> GetAllProductsAsync()
        {
            try
            {
                var serviceResponse = await _productService.GetAllProductsAsync();
                if (!serviceResponse.Success)
                {
                    return BadRequest(serviceResponse);
                }
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("GetProductById/{id:int}")]
        public async Task<ActionResult<ServiceResponse<ProductDTO>>> GetProductByIdAsync(int id)
        {
            try
            {
                var serviceResponse = await _productService.GetProductByIdAsync(id);
                if (!serviceResponse.Success)
                {
                    return NotFound(serviceResponse);
                }
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching product with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateProductAsync([FromBody] ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var serviceResponse = await _productService.CreateProductAsync(product);
                if (!serviceResponse.Success)
                {
                    return BadRequest(serviceResponse);
                }
                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = serviceResponse.Data }, serviceResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a product.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("UpdateProduct/{id:int}")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateProductAsync(int id, [FromBody] ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var serviceResponse = await _productService.UpdateProductAsync(product);
                if (!serviceResponse.Success)
                {
                    return NotFound(serviceResponse);
                }
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("DeleteProduct/{id:int}")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteProductAsync(int id)
        {
            try
            {
                var serviceResponse = await _productService.DeleteProductAsync(id);
                if (!serviceResponse.Success)
                {
                    return NotFound(serviceResponse);
                }
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
