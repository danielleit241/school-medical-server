using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Account;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Account
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController(IAccountServices accountService) : ControllerBase
    {
        [HttpPost("register-staff")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult?> RegisterStaff([FromBody] RegisterRequestDto request)
        {
            var user = await accountService.RegisterStaffAsync(request);
            if (user is null)
                return BadRequest("Registration failed");

            return Ok(user);
        }
    }
}
