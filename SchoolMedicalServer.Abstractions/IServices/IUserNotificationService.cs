using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUserNotificationService
    {
        Task<PaginationResponse<NotificationResponse>> GetUserNotificationsAsync(PaginationRequest? request, Guid userId);
        Task<int> GetUserUnReadNotificationsAsync(Guid? userId);
        Task<NotificationResponse> GetUserNotificationDetailsAsync(Guid notificationId);
        Task<bool> ReadAllNotificationsAsync(Guid userId);
    }
}
