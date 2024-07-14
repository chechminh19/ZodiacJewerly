using Application.IService;
using Application.Utils.PaymentTypes;
using Application.ViewModels.OrderDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("Allow")]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly PayOS _payOS;

        public OrderController(IOrderService orderService, PayOS payOs)
        {
            _orderService = orderService;
            _payOS = payOs;
        }

        [HttpPost("{userid}/{productid}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddProductToOrder(int userid, int productid)
        {
            var result = await _orderService.AddProductToOrderAsync(userid, productid);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("customer/{userid}")]
        public async Task<IActionResult> GetAllOrderCartCustomer(int userid)
        {
            var result = await _orderService.GetAllOrderCustomerCart(userid);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet("order/{orderid}")]
        public async Task<IActionResult> GetAllOrderDetailById(int orderid)
        {
            var result = await _orderService.GetAllOrderDetailById(orderid);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Staff,Admin,Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 5,
            [FromQuery] string search = "", [FromQuery] string status = "", [FromQuery] string sort = "id")
        {
            var result = await _orderService.GetAllOrder(page, pageSize, search, status, sort);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Staff,Admin,Customer")]
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

        [Authorize(Roles = "Customer")]
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

        [HttpPut("update-quantity")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            var result = await _orderService.UpdateOrderQuantity(request.OrderId, request.ProductId, request.Quantity);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("remove-product/{orderId}/{productId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveProduct(int orderId, int productId)
        {
            var result = await _orderService.RemoveProductFromCart(orderId, productId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [Authorize(Roles = "Staff,Admin")]
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

        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentLink(CreatePaymentLinkRequest body)
        {
            try
            {
                var cartServiceResponse = await _orderService.GetAllOrderCustomerCart(body.userId);
                if (!cartServiceResponse.Success || cartServiceResponse == null)
                {
                    if (cartServiceResponse.Message == "No order here")
                    {
                        return Ok(new ResponsePayment(0, "success", new { message = "No items in cart" }));
                    }

                    return Ok(new ResponsePayment(-1, "fail", cartServiceResponse.Error));
                }

                var items = cartServiceResponse.Data.Product
                    .Select(productDTO =>
                        new ItemData(productDTO.NameProduct,
                            productDTO.Quantity,
                            (int)productDTO.Price)).ToList();
                var orderCode = (long)cartServiceResponse.Data.Product.FirstOrDefault()?.OrderId;
                //int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));             

                PaymentData paymentData = new PaymentData(orderCode, (int)cartServiceResponse.Data.PriceTotal,
                    body.description, items, body.cancelUrl, body.returnUrl);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                return Ok(new ResponsePayment(0, "success", createPayment));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new ResponsePayment(-1, "fail", null));
            }
        }

        [HttpGet("payment/{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] long orderId)
        {
            try
            {
                var paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                return Ok(new ResponsePayment(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new ResponsePayment(-1, "fail", null));
            }
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute] long orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderId);
                return Ok(new ResponsePayment(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new ResponsePayment(-1, "fail", null));
            }
        }

        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook(ConfirmWebhook body)
        {
            try
            {
                await _payOS.confirmWebhook(body.webhook_url);
                return Ok(new ResponsePayment(0, "Ok", null));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new ResponsePayment(-1, "fail", null));
            }
        }

        [HttpGet("sales-by-item")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSalesByItem()
        {
            var result = await _orderService.GetSalesByItemAsync();
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpGet("sales-overview")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSalesOverview([FromQuery] int? year)
        {
            if (!year.HasValue)
            {
                return BadRequest("Please input the specific year.");
            }

            var result = await _orderService.GetSalesOverviewAsync(year.Value);
            if (result.Success) return Ok(result);
            if (result.Message == "No sales data found for the specified year.")
                return NotFound(result.Message);

            return BadRequest(result.Message);
        }
    }
}