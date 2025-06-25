using SchoolMedicalServer.Abstractions.Dtos.Notification;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAppointmentNotificationService
    {
        Task<NotificationResponse> SendAppointmentNotificationToNurseAsync(NotificationRequest request);
        Task<NotificationResponse> SendAppointmentNotificationToParentAsync(NotificationRequest request);
    }
}
