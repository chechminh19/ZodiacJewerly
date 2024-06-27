using Application.IService;
using Application.Services;
using Application.ViewModels.OrderDTO;
using Application.ViewModels.ProductDTO;
using Application.ViewModels.UserDTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ZodiacJewelryWebApI.Controllers
{
    [EnableCors("Allow")]
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int page)
        {
            var result = await _userService.GetAllUsers(page);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetAllUsersCustomer(int page)
        {
            var result = await _userService.GetAllUsersByCustomer( page);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetAllUsersAdmin(int page)
        {
            var result = await _userService.GetAllUsersByAdmin(page);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("staff")]
        public async Task<IActionResult> GetAllUsersStaff(int page)
        {
            var result = await _userService.GetAllUsersByStaff(page);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetAllUsersByRole(string role,int page)
        {
            var result = await _userService.GetAllUsersByRole(role,page);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserById(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDTO userDTO)
        {
            var result = await _userService.AddUser(userDTO);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser( [FromBody] UserDTO userDTO)
        {
         

            var result = await _userService.UpdateUser(userDTO);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
