using SchoolMedicalServer.Abstractions.Dtos.Notification;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalRegistrationNotificationService
    {
        Task<NotificationResponse> SendMedicalRegistrationNotificationToNurseAsync(NotificationRequest request);
        Task<NotificationResponse> SendMedicalRegistrationApprovedNotificationToParentAsync(NotificationRequest request);
        Task<NotificationResponse> SendMedicalRegistrationCompletedNotificationToParentAsync(NotificationRequest request);
    }
}
