using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Helpers;
using SchoolMedicalServer.Abstractions.Dtos.Account;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Helpers.EmailHelper;

namespace SchoolMedicalServer.Api.Controllers.Account
{
    [Route("api")]
    [ApiController]
    public class AccountController(IAccountService accountService, IEmailHelper emailHelper, IWebHostEnvironment _env) : ControllerBase
    {
        [HttpPost("accounts/register-staff")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult?> RegisterStaff([FromBody] RegisterStaffRequest request)
        {
            var account = await accountService.RegisterStaffAsync(request);
            if (account is null)
                return BadRequest("Registration failed");

            string templatePath = Path.Combine(_env.WebRootPath, "templates", "register_staff_email_template.html");

            string htmlBody = await System.IO.File.ReadAllTextAsync(templatePath);

            htmlBody = htmlBody.Replace("{PHONENUMBER}", account.PhoneNumber)
                   .Replace("{PASSWORD}", account.Password)
                   .Replace("{FULLNAME}", account.FullName);

            var emailDesc = new EmailDto
            {
                To = request.Email,
                Subject = "Batch Parent Account Creation",
                Body = htmlBody
            };

            await emailHelper.SendEmailAsync(emailDesc);

            return Ok(account);
        }

        [HttpPost("accounts/parents/batch-create")]
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
