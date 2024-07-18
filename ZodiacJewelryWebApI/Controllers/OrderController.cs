using Application.IService;
using Application.ViewModels.OrderDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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

        #region Order Operations
        /// <summary>
        /// Adds a product to an order for a specific user.
        /// </summary>
        [Authorize(Roles = "Customer")]
        [HttpPost("{userid}/{productid}")]
        public async Task<IActionResult> AddProductToOrder(int userid, int productid)
        {
            var result = await _orderService.AddProductToOrderAsync(userid, productid);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderDTO orderDTO)
        {
            var result = await _orderService.AddOrder(orderDTO);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Updates the quantity of a product in an order.
        /// </summary>
        [Authorize(Roles = "Customer")]
        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            var result = await _orderService.UpdateOrderQuantity(request.OrderId, request.ProductId, request.Quantity);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Removes a product from an order.
        /// </summary>
        [Authorize(Roles = "Customer")]
        [HttpDelete("remove-product/{orderId}/{productId}")]
        public async Task<IActionResult> RemoveProduct(int orderId, int productId)
        {
            var result = await _orderService.RemoveProductFromCart(orderId, productId);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Completes the payment for an order and sets its status to completed.
        /// </summary>
        [Authorize(Roles = "Customer")]
        [HttpPut("{orderid}/complete-payment")]
        public async Task<IActionResult> PaymentOrder(int orderid)
        {
            var result = await _orderService.PaymentOrder(orderid);
            if (!result.Success) return NotFound(result);

            return Ok(result);
        }

        #endregion

        #region Order Retrieval
        /// <summary>
        /// Retrieves all orders for a specific customer.
        /// </summary>
        [HttpGet("customer/{userid}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAllOrderCartCustomer(int userid)
        {
            var result = await _orderService.GetAllOrderCustomerCart(userid);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves order details for a specific order ID.
        /// </summary>
        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet("order/{orderid}")]
        public async Task<IActionResult> GetAllOrderDetailById(int orderid)
        {
            var result = await _orderService.GetAllOrderDetailById(orderid);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves all orders 
        /// </summary>
        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 5,
            [FromQuery] string search = "", [FromQuery] string status = "", [FromQuery] string sort = "id")
        {
            var result = await _orderService.GetAllOrder(page, pageSize, search, status, sort);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        #endregion

        #region Sales Overview
        /// <summary>
        /// Retrieves sales data grouped by item.
        /// </summary>
        [HttpGet("sales-by-item")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSalesByItem()
        {
            var result = await _orderService.GetSalesByItemAsync();
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a sales overview for a specific year.
        /// </summary>
        [HttpGet("sales-overview")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSalesOverview([FromQuery] int? year)
        {
            if (!year.HasValue)
            {
                return BadRequest("Please input the specific year.");
            }

            var result = await _orderService.GetSalesOverviewAsync(year.Value);
            if (!result.Success)
            {
                if (result.Message == "No sales data found for the specified year.")
                    return NotFound(result.Message);

                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        #endregion
    }
}