namespace SchoolMedicalServer.Api.Helpers
{
    public interface INotificationSender
    {
        Task NotifyUserUnreadCountAsync(Guid? userId);
    }
}
