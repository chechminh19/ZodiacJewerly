using Application.IService;
using Application.Services;
using Application.Utils.PaymentTypes;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using Azure;

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

                PaymentData paymentData = new PaymentData(orderCode, (int)cartServiceResponse.Data.PriceTotal, body.description, items, body.cancelUrl, body.returnUrl);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                return Ok(new ResponsePayment(0, "success", createPayment));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new ResponsePayment(-1, "fail", null));
            }
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] long orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
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
    }
}
