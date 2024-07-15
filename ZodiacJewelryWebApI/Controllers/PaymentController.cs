using Azure;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Application.Utils.PaymentTypes;
using Domain.Entities;
using Application.IService;
using Application.ViewModels.OrderDTO;
using Application.Services;
namespace ZodiacJewelryWebApI.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly PayOS _payOS;
        private readonly IOrderService _orderService;
        public PaymentController(PayOS payOS, IOrderService orderService)
        {
            _payOS = payOS;
            _orderService = orderService;

        }
        //[HttpPost("payos_transfer_handler")]
        //public IActionResult payOSTransferHandler(WebhookType body)
        //{
        //    try
        //    {
        //        WebhookData data = _payOS.verifyPaymentWebhookData(body);

        //        if (data.description == "Ma giao dich thu nghiem" || data.description == "VQRIO123")
        //        {
        //            long orderCode = data.orderCode;

        //            _orderService.UpdateOrderStatusToPaid(orderCode);
        //            return Ok(new ResponsePayment(0, "Ok", null));
        //        }
        //        return Ok(new ResponsePayment(0, "Ok", null));
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return Ok(new ResponsePayment(-1, "fail", null));
        //    }
        //}
        [HttpPost("create-payment-link")]
        public async Task<IActionResult> Checkout(CreatePaymentLinkRequest body)
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
                PaymentData paymentData = new PaymentData(orderCode, (int)cartServiceResponse.Data.PriceTotal, "Thanh toan don hang", items, "https://zodiacgems.vercel.app/cancel", "https://zodiacgems.vercel.app/success");

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                return Redirect(createPayment.checkoutUrl);
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Redirect("https://zodiacgems.vercel.app");
            }
        }
    }
}
