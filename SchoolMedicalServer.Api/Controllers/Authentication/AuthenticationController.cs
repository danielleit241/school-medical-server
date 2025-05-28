using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Authentication;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Authentication
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(IAuthService authService) : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult?> Login([FromBody] LoginRequest request)
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

        [HttpPost("change-password")]
        public async Task<IActionResult?> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await authService.ChangePasswordAsync(request);
            if (user is null)
                return BadRequest("Change Password Fail");

            return Ok(user);
        }
    }
}
