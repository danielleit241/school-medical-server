using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Services;

namespace SchoolMedicalServer.Api.Controllers.Authentication
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult?> Register([FromBody] UserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user is null)
                return BadRequest("Registration failed");

            return Ok(user);
        }

        [HttpGet("is-auth")]
        [Authorize]
        public IActionResult IsAuthenticated()
        {
            return Ok("User is authenticated");
        }

        [HttpGet("is-admin")]
        [Authorize]
        public IActionResult IsAdminAuthenticated()
        {
            return Ok("Admin is authenticated");
        }

        [HttpPost("login")]
        public async Task<IActionResult?> Login([FromBody] UserDto request)
        {
            var token = await authService.LoginAsync(request);
            if (token is null)
                return BadRequest("Invalid credentials");

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult?> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var token = await authService.RefreshTokenAsync(request);
            if (token is null)
                return BadRequest("Invalid refresh token");

            return Ok(token);
        }
    }
}
