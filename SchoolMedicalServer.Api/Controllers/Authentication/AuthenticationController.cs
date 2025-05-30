using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Authentication;
using SchoolMedicalServer.Abstractions.Dtos.Helpers;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Helpers.EmailHelper;

namespace SchoolMedicalServer.Api.Controllers.Authentication
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(IAuthService authService, IEmailHelper helper, IWebHostEnvironment _env) : ControllerBase
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

        [HttpPost("forgot-password/send-otp")]
        public async Task<IActionResult?> SendOtp([FromBody] SendOtpRequest request)
        {
            if (request is null || string.IsNullOrEmpty(request.PhoneNumber))
                return BadRequest("Invalid forgot password request");

            string otp = await authService.GetOtpAsync(request);
            if (string.IsNullOrEmpty(otp))
                return BadRequest("Failed to send OTP");

            string templatePath = Path.Combine(_env.WebRootPath, "templates", "otp_template.html");
            if (!System.IO.File.Exists(templatePath))
                return NotFound("Email template not found");

            string htmlBody = await System.IO.File.ReadAllTextAsync(templatePath);
            htmlBody = htmlBody.Replace("{OTP}", otp);
            var email = new EmailDto
            {
                To = request.EmailAddress,
                Subject = "Your Password Reset OTP",
                Body = htmlBody
            };

            await helper.SendEmailAsync(email);
            return Ok("Forgot password request processed successfully");
        }


        [HttpPost("forgot-password/verify-otp")]
        public async Task<IActionResult?> VerifyOtp([FromBody] string otp)
        {
            if (otp is null || string.IsNullOrEmpty(otp))
                return BadRequest("Invalid verify OTP request");
            bool isValid = await authService.VerifyOtpAsync(otp);
            if (!isValid)
                return BadRequest("Invalid OTP");
            return Ok("OTP verified successfully");
        }

        [HttpPost("forgot-password/reset-password")]
        public async Task<IActionResult?> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request is null || string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.NewPassword) ||
                string.IsNullOrEmpty(request.ConfirmNewPassword))
                return BadRequest("Invalid reset password request");
            bool isReset = await authService.ResetPasswordAsync(request);
            if (!isReset)
                return BadRequest("Reset Password Fail");
            return Ok("Password reset successfully");
        }
    }
}
