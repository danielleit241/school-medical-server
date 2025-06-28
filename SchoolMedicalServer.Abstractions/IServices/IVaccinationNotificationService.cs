using SchoolMedicalServer.Abstractions.Dtos.Notification;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationNotificationService
    {
        Task<IEnumerable<NotificationResponse>> SendVaccinationResultNotificationToParents(IEnumerable<NotificationRequest> requests);
        Task<IEnumerable<NotificationResponse>> SendVaccinationNotificationToNurses(IEnumerable<NotificationRequest> requests);
        Task<NotificationResponse> SendVaccinationObservationNotificationToParent(NotificationRequest requests);
        Task<NotificationResponse> SendVaccinationResultNotificationToParent(NotificationRequest request);
    }
}
