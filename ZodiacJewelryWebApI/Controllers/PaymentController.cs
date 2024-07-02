using Azure;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using Application.Utils.PaymentTypes;
using Domain.Entities;
using Application.IService;
using Application.ViewModels.OrderDTO;
namespace ZodiacJewelryWebApI.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly PayOS _payOS;
        public PaymentController(PayOS payOS)
        {
            _payOS = payOS;
        }
        [HttpPost("payos_transfer_handler")]
        public IActionResult payOSTransferHandler(WebhookType body)
        {
            try
            {
                WebhookData data = _payOS.verifyPaymentWebhookData(body);

                if (data.description == "Ma giao dich thu nghiem" || data.description == "VQRIO123")
                {
                    return Ok(new ResponsePayment(0, "Ok", null));
                }
                return Ok(new ResponsePayment(0, "Ok", null));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Ok(new ResponsePayment(-1, "fail", null));
            }
        }
    }
}
