using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SchoolMedicalServer.Abstractions.Dtos.Helpers;

namespace SchoolMedicalServer.Api.Helpers.EmailHelper
{
    public class EmailHelper(IConfiguration configuration) : IEmailHelper
    {
        public async Task SendEmailAsync(EmailDto request)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(configuration["EmailUsername"]));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };
                email.Headers.Add("X-Priority", "3");
                email.Headers.Add("X-Mailer", "ASP.NET Web API Mailer");
                email.Date = DateTimeOffset.Now;


                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(configuration["EmailHost"], int.Parse(configuration["EmailPort"]!), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(configuration["EmailUsername"], configuration["EmailPassword"]);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                throw;
            }
        }

    }
}
