using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface INotificationService
    {
        Task<PaginationResponse<NotificationResponse>> GetUserNotificationsAsync(PaginationRequest? request, Guid userId);

        Task<NotificationResponse> GetAppoimentNotificationAsync(Guid notificationId);
        //Task<PaginationResponse<NotificationResponse>> GetAppoimentNotificationsByUserAsync(PaginationRequest pagination, Guid userId);
        Task<NotificationResponse> SendAppoimentNotificationToNurseAsync(NotificationRequest request);
        Task<NotificationResponse> SendAppoimentNotificationToParentAsync(NotificationRequest request);

        Task<NotificationResponse> SendMedicalRegistrationApprovedNotificationToParentAsync(NotificationRequest request);
        Task<NotificationResponse> SendMedicalRegistrationCompletedNotificationToParentAsync(NotificationRequest request);
        Task<NotificationResponse> GetMedicalRegistrationNotificationAsync(Guid notificationId);
        //Task<PaginationResponse<NotificationResponse>> GetMedicalRegistrationNotificationsAsync(PaginationRequest pagination, Guid userId);


        Task<NotificationResponse> SendMedicalEventNotificationToParentAsync(NotificationRequest request);
        Task<NotificationResponse> GetMedicalEventNotificationAsync(Guid notificationTypeId);
        Task<bool> ReadAllNotificationsAsync(Guid userId);
        Task<int> GetUserUnReadNotificationsAsync(Guid? userId);
    }
}
