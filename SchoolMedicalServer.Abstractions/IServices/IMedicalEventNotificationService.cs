using SchoolMedicalServer.Abstractions.Dtos.Notification;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalEventNotificationService
    {
        Task<NotificationResponse> SendMedicalEventNotificationToParentAsync(NotificationRequest request);
    }
}
