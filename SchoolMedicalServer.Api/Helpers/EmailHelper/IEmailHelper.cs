using SchoolMedicalServer.Abstractions.Dtos;

namespace SchoolMedicalServer.Api.Helpers.EmailHelper
{
    public interface IEmailHelper
    {
        Task SendEmailAsync(EmailDto request);
    }
}
