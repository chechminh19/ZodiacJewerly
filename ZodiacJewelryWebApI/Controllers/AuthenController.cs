using Application.IService;
using Application.ViewModels.UserDTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("http://localhost:4200")]
    public class AuthenController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenController(IAuthenticationService authen)
        {
            _authenticationService = authen;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDTO registerObject)
        {
            var result = await _authenticationService.RegisterAsync(registerObject);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
