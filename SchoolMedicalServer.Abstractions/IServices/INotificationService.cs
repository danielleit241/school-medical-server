using SchoolMedicalServer.Abstractions.Dtos;
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
    }
}
