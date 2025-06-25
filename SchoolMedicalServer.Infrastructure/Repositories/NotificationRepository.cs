using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class NotificationRepository(SchoolMedicalManagementContext _context) : INotificationRepository
    {
        public async Task<int> CountByUserIdAsync(Guid userId)
            => await _context.Notifications.Where(n => n.UserId == userId).CountAsync();

        public async Task<List<Notification>> GetByUserIdPagedAsync(Guid userId, int skip, int take)
            => await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.SendDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

        public async Task<Notification?> GetByIdAsync(Guid notificationId)
            => await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == notificationId);

        public async Task<List<Notification>> GetUnreadByUserIdAsync(Guid userId)
            => await _context.Notifications.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();

        public async Task<int> CountUnreadByUserIdAsync(Guid userId)
            => await _context.Notifications.Where(n => n.UserId == userId && !n.IsRead).CountAsync();

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<Notification> notifications)
        {
            _context.Notifications.UpdateRange(notifications);
            await _context.SaveChangesAsync();
        }
    }
}
