using Application.IService;
using Application.ViewModels.UserDTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("Allow")]
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
        [HttpPost("ForgotPassword/{email}")]
        public async Task<IActionResult> ForgotPassword(ForgotPassDTO user)
        {
            try
            {
                var result = await _authenticationService.ForgotPass(user);
                if (!result.Success)
                {
                    return StatusCode(401, result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
