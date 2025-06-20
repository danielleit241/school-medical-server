using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Helpers;
using SchoolMedicalServer.Abstractions.Dtos.Account;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Helpers;

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
            if (request is null)
                return BadRequest("Invalid request data");
            if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.FullName) ||
                string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) ||
                string.IsNullOrEmpty(request.RoleName))
            {
                return BadRequest("All fields are required");
            }

            var account = await accountService.RegisterStaffAsync(request);
            if (account is null)
                return BadRequest("Registration failed");

            string templatePath = Path.Combine(_env.WebRootPath, "templates", "register_email_template.html");
            if (!System.IO.File.Exists(templatePath))
                return NotFound("Email template not found");

            await SendEmail(account, templatePath);

            return Ok(account);
        }

        private async Task SendEmail(AccountResponse account, string templatePath)
        {
            string htmlBody = await System.IO.File.ReadAllTextAsync(templatePath);

            htmlBody = htmlBody.Replace("{PHONENUMBER}", account.PhoneNumber)
                   .Replace("{PASSWORD}", account.Password)
                   .Replace("{FULLNAME}", account.FullName);

            var emailDesc = new EmailFrom
            {
                To = account.EmailAddress,
                Subject = "Account Creation",
                Body = htmlBody
            };

            await emailHelper.SendEmailAsync(emailDesc);
        }

        [HttpPost("accounts/parents/batch-create")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> BatchCreateParents()
        {
            var accounts = await accountService.BatchCreateParentsAsync();
            if (accounts is null)
                return BadRequest("No accounts created");

            string templatePath = Path.Combine(_env.WebRootPath, "templates", "register_email_template.html");
            if (!System.IO.File.Exists(templatePath))
                return NotFound("Email template not found");

            var semaphore = new SemaphoreSlim(5);
            var tasks = accounts.Select(async account =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await SendEmail(account, templatePath);
                }
                finally
                {
                    semaphore.Release();
                }
            });
            await Task.WhenAll(tasks);

            return Ok(accounts);
        }
    }
}
