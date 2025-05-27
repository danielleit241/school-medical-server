using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Helpers;
using SchoolMedicalServer.Abstractions.Dtos.Account;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Helpers.EmailHelper;

namespace SchoolMedicalServer.Api.Controllers.Account
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController(IAccountServices accountService, IEmailHelper emailHelper) : ControllerBase
    {
        [HttpPost("register-staff")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult?> RegisterStaff([FromBody] RegisterStaffRequest request)
        {
            var account = await accountService.RegisterStaffAsync(request);
            if (account is null)
                return BadRequest("Registration failed");

            var emailDesc = new EmailDto
            {
                To = "hoalvpse181951@fpt.edu.vn",
                Subject = "Batch Parent Account Creation",
                Body = $@"
                <html>
                <body>
                    <h2>Xin chào!</h2>
                    <p>{"Bạn đã nhận được email từ hệ thống quản lý y tế."}</p>
                    <p>Trân trọng,<br/>Đội ngũ phát triển</p>
                </body>
                </html>"
            };

            await emailHelper.SendEmailAsync(emailDesc);

            return Ok(account);
        }

        [HttpPost("parents/batch-create")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> BatchCreateParents()
        {
            var accounts = await accountService.BatchCreateParentsAsync();
            if (accounts is null)
                return BadRequest("No accounts created");

            return Ok(accounts);
        }
    }
}
