using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface INotificationService
    {
        Task<NotificationResponse> GetAppoimentNotificationAsync(Guid notificationId);
        Task<PaginationResponse<NotificationResponse>> GetAppoimentNotificationsByUserAsync(PaginationRequest pagination, Guid userId);

        Task<NotificationResponse> SendAppoimentToNurseNotificationAsync(NotificationRequest request);
        Task<NotificationResponse> SendAppoimentToParentNotificationAsync(NotificationRequest request);
    }
}
