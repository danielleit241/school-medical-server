using SchoolMedicalServer.Abstractions.Dtos.Helpers;

namespace SchoolMedicalServer.Api.Helpers.EmailHelper
{
    public interface IEmailHelper
    {
        Task SendEmailAsync(EmailDto request);
    }
}
