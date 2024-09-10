using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WEB.DTOs;
using WEB.Interface.IRepository;
using WEB.Models.Common;

namespace WEB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDTO model)
        {
            var user = await _userRepository.UserLogin(model);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }
            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO model)
        {
            var existingUser = await _userRepository.FindUser(model.Username);

            if (existingUser != null)
            {
                return BadRequest(new { Message = "username already exists, failed to register." });
            }

            await _userRepository.UserRegister(model);
            return Ok(new { Message = $"user registered with username {model.Username}" });

        }

        [Authorize]
        [HttpGet("GetUserInfo/{id}")]
        public async Task<IActionResult> GetUserInfo(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}