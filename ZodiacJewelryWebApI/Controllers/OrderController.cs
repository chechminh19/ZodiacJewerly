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
    [Route("api/orders")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrder();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderById(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
                 
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderDTO orderDTO)
        {
            var result = await _orderService.AddOrder(orderDTO);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder( [FromBody] OrderDTO orderDTO)
        {
         

            var result = await _orderService.UpdateOrder(orderDTO);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrder(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
