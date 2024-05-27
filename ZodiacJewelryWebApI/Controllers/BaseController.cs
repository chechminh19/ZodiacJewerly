using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHttpCors = System.Web.Http.Cors;
namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("https://zodiacjewelrywebapi.azurewebsites.net")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
