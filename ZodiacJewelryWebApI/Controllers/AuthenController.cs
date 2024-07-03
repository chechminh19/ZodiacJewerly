using Application.IService;
using Application.ViewModels.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("Allow")]
    [Route("api/authentication")]
    [ApiController]
    public class AuthenController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        //private Dictionary<string, (string, DateTime)> emailVerifyCode = new Dictionary<string, (string, DateTime)>();
        public AuthenController(IAuthenticationService authen)
        {
            _authenticationService = authen;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerObject)
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

        [Authorize(Roles = "Admin")]
        [HttpPost("staff")] //Admin
        public async Task<IActionResult> NewAccountStaff(RegisterDTO registerObject)
        {
            var result = await _authenticationService.CreateStaff(registerObject);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _authenticationService.ForgotPass(email);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOTPResetDTO request)
        {
            var response = await _authenticationService.VerifyForgotPassCode(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassWord(ResetPassDTO dto)
        {
            var response = await _authenticationService.ResetPass(dto);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
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
                        token = result.DataToken,
                        role = result.Role,
                        hint = result.HintId,
                    }
                );
            }
        }
    }
}