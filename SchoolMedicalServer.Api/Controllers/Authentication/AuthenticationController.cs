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
            if (request is null || string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Invalid login request");

            var token = await authService.LoginAsync(request);
            if (token is null)
                return BadRequest("Invalid credentials");

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult?> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (request is null || request.UserId == Guid.Empty || string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest("Invalid refresh token request");

            var token = await authService.RefreshTokenAsync(request);
            if (token is null)
                return BadRequest("Invalid refresh token");

            return Ok(token);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult?> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (request is null || string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.OldPassword) ||
                string.IsNullOrEmpty(request.NewPassword) || string.IsNullOrEmpty(request.ConfirmNewPassword))
                return BadRequest("Invalid change password request");
            if (request.NewPassword != request.ConfirmNewPassword)
                return BadRequest("New password and confirm password do not match");
            var user = await authService.ChangePasswordAsync(request);
            if (user is null)
                return BadRequest("Change Password Fail");

            return Ok(user);
        }
    }
}
