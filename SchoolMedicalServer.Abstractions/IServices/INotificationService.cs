using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface INotificationService
    {
        Task<PaginationResponse<NotificationResponse>> GetUserNotificationsAsync(PaginationRequest? request, Guid userId);

        Task<NotificationResponse> SendAppoimentNotificationToNurseAsync(NotificationRequest request);
        Task<NotificationResponse> SendAppoimentNotificationToParentAsync(NotificationRequest request);

        Task<NotificationResponse> SendMedicalRegistrationApprovedNotificationToParentAsync(NotificationRequest request);
        Task<NotificationResponse> SendMedicalRegistrationCompletedNotificationToParentAsync(NotificationRequest request);

        Task<NotificationResponse> SendMedicalEventNotificationToParentAsync(NotificationRequest request);
        Task<bool> ReadAllNotificationsAsync(Guid userId);
        Task<int> GetUserUnReadNotificationsAsync(Guid? userId);
        Task<NotificationResponse> GetUserNotificationDetailsAsync(Guid notificationId);
        Task<NotificationResponse> SendMedicalRegistrationNotificationToNurseAsync(NotificationRequest request);


        Task<IEnumerable<NotificationResponse>> SendVaccinationNotificationToParents(IEnumerable<NotificationRequest> requests);
        Task<IEnumerable<NotificationResponse>> SendVaccinationNotificationToNurses(IEnumerable<NotificationRequest> requests);
        Task<NotificationResponse> SendVaccinationObservationNotificationToParent(NotificationRequest requests);
    }
}
