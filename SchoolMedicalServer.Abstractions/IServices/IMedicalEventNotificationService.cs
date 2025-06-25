using SchoolMedicalServer.Abstractions.Dtos.Notification;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalEventNotificationService
    {
        Task<NotificationResponse> SendMedicalEventNotificationToManagerAsync(NotificationRequest request);
        Task<NotificationResponse> SendMedicalEventNotificationToParentAsync(NotificationRequest request);
    }
}
