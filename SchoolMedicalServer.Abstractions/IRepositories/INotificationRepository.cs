using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface INotificationRepository
    {
        Task<int> CountByUserIdAsync(Guid userId);
        Task<List<Notification>> GetByUserIdPagedAsync(Guid userId, int skip, int take);
        Task<Notification?> GetByIdAsync(Guid notificationId);
        Task<List<Notification>> GetUnreadByUserIdAsync(Guid userId);
        Task<int> CountUnreadByUserIdAsync(Guid userId);
        Task AddAsync(Notification notification);
    }
}
