using Application.IService;
using Application.ViewModels.UserDTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers
{
    
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


        //[Authorize(Roles = "Staff")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginUserDTO loginObject)
        {
            var result = await _authenticationService.LoginAsync(loginObject);

            if (!result.Success)
            {
                return StatusCode(401, result);
            }
            else
            {
                return Ok(
                    new
                    {
                        success = result.Success,
                        message = result.Message,
                        token = result.Data
                    }
                );
            }
        }
    }
}
