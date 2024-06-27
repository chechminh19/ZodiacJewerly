using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public PaymentController(IConfiguration config, IHttpContextAccessor con)
        {
            _configuration = config;
            _contextAccessor = con;
        }
        //public Task<IActionResult> PaymentWithPaypal(string cancel = null, string blogId = "",
        //    string PayerId ="", string guid ="")
        //{
        //    var clientId = _configuration["PayPal:ClientId"];
        //    var clientSecret = _configuration["PayPal:ClientSecret"];
        //    var mode = _configuration["PayPal:mode"];
        //    APIContext apiContext = PayPalConfiguration.GetAPIContext(clientId, clientSecret, mode);
        //    try
        //    {
        //        string payerId = PayerId;
        //        if (string.IsNullOrEmpty(payerId))
        //        {
        //            string baseURL = this.Request.Scheme + "://" + this.Request.Host + "/Home/PaymentWithPayPal?";
        //            var guidd = Convert.ToString((new Random().Next(100000)));
        //            guid = guidd;
        //            var createdPayment = this.CreatePayment(apiContext, baseURL + "guid=" + guid, blogId);
        //            var links = createdPayment.links.GetEnumerator();
        //            string paypalRedirectUrl = null;
        //            while (links.MoveNext())
        //            {
        //                Links link = links.Current;
        //                if (link.rel.ToLower().Trim().Equals("approval_url"))
        //                {
        //                    paypalRedirectUrl = link.href;
        //                }
        //            }
        //            _contextAccessor.HttpContext.Session.SetString("payment", createdPayment.id);
        //            return Redirect(paypalRedirectUrl);
        //        }
        //        else
        //        {
        //            var paymentId = _contextAccessor.HttpContext.Session.GetString("payment");
        //            var excutedPayment = ExecutePayment(apiContext, payerId, paymentId as string);
        //            if(excutedPayment.state.ToLower() != "approved")
        //            {
        //                return Redirect("/");
        //            }
        //            var blogIds = excutedPayment.transactions[0].item_list.items[0].sku;
        //            return Redirect("/");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Redirect("/");
        //    }
        //    return Redirect("/");
        //}
        //private PayPal.Api.Payment payment;
        //private Payment ExecutePayment(APIContext aPIContext, string payerId, string paymentId)
        //{
        //    var paymentExecution = new PaymentExecution()
        //    {
        //        payer_id = payerId,
        //    };
        //    this.payment = new Payment()
        //    {
        //        id = paymentId,
        //    };
        //    return this.payment.Execute(aPIContext, paymentExecution);
        //}
        //private Payment CreatePayment(APIContext apiContext, string redirectUrl, string blogId)
        //{
        //    var itemList = new ItemList()
        //    {
        //        items = new List<Item>()
        //    };
        //    itemList.items.Add(new Item()){
        //        name = "aaa";
        //        price = "100";
        //    };
        //    var payer = new Payer()
        //    {
        //        payment_method = "paypal"
        //    };
        //    var redirctUrls = new RedirectUrls()
        //    {
        //        cancel_url = redirectUrl + "&Cancel=true",
        //        return_url = redirectUrl
        //    };
        //    var transactionList = new List<Transaction>();
        //    transactionList.Add(new Transaction()
        //    {
        //        description = "oke",
        //        amount = amount,
        //        item_list = itemList,
        //    });
        //    this.payment = new Payment()
        //    {
        //        intent = "sale",
        //        payer = payer,
        //        transactions = transactionList,
        //        redirect_urls = redirctUrls
        //    };
        //    return this.payment.Create(apiContext);
        //}
    }

}
