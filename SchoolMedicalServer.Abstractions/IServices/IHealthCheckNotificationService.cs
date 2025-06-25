using SchoolMedicalServer.Abstractions.Dtos.Notification;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IHealthCheckNotificationService
    {
        Task<IEnumerable<NotificationResponse>> SendHealthCheckNotificationToParents(IEnumerable<NotificationRequest> requests);
        Task<IEnumerable<NotificationResponse>> SendHealthCheckNotificationToNurses(IEnumerable<NotificationRequest> requests);
        Task<NotificationResponse> SendHealthCheckResultNotificationToParent(NotificationRequest request);
    }
}
