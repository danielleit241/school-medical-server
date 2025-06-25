namespace SchoolMedicalServer.Api.Helpers
{
    public interface IEmailHelper
    {
        Task SendEmailAsync(EmailFrom request);
    }
}
