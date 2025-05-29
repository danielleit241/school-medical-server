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
    public class AccountController(IAccountService accountService, IEmailHelper emailHelper) : ControllerBase
    {
        [HttpPost("register-staff")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult?> RegisterStaff([FromBody] RegisterStaffRequest request)
        {
            var account = await accountService.RegisterStaffAsync(request);
            if (account is null)
                return BadRequest("Registration failed");

            string htmlBody = System.IO.File.ReadAllText("register_staff_email_template.html");

            htmlBody = htmlBody.Replace("{PHONENUMBER}", account.PhoneNumber)
                   .Replace("{PASSWORD}", account.Password)
                   .Replace("{fullName}", account.FullName);

            var emailDesc = new EmailDto
            {
                To = request.Email,
                Subject = "Batch Parent Account Creation",
                Body = htmlBody
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
