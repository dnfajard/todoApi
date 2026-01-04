using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            var result = await _authService.Register(request);

            if (result == null)
            {
                return BadRequest(new { message = "User already exists" });
            }

            var (user, token) = result.Value;

            return Ok(new 
            { 
                token, 
                user = new { user.Id, user.Email, user.Name } 
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email and password are required" });
            }

            var result = await _authService.Login(request);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var (user, token) = result.Value;

            return Ok(new 
            { 
                token, 
                user = new { user.Id, user.Email, user.Name } 
            });
        }
    }
}