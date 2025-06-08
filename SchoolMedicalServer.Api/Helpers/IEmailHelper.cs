using SchoolMedicalServer.Abstractions.Dtos.Helpers;

namespace SchoolMedicalServer.Api.Helpers
{
    public interface IEmailHelper
    {
        Task SendEmailAsync(EmailFrom request);
    }
}
