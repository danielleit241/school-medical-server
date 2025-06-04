using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface INotificationService
    {
        Task<NotificationResponse> GetAppoimentNotificationAsync(Guid notificationId);
        Task<PaginationResponse<NotificationResponse>> GetAppoimentNotificationsByUserAsync(PaginationRequest pagination, Guid userId);

        Task<NotificationResponse> SendAppoimentToNurseNotificationAsync(NotificationRequest request);
        Task<NotificationResponse> SendAppoimentToParentNotificationAsync(NotificationRequest request);

        Task<NotificationResponse> SendMedicalRegistrationNotificationToParentAsync(NotificationRequest request);
        Task<NotificationResponse> SendMedicalRegistrationDetailsNotificationToParentAsync(NotificationRequest request);    
        Task<NotificationResponse> GetMedicalRegistrationNotificationAsync(Guid notificationId);
        Task<PaginationResponse<NotificationResponse>> GetMedicalRegistrationNotificationsAsync(PaginationRequest pagination, Guid userId);
    }
}
