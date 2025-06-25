using Microsoft.AspNetCore.SignalR;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Hubs;

namespace SchoolMedicalServer.Api.Helpers
{
    public class NotificationSender(IUserNotificationService service, IHubContext<NotificationHub> hubContext) : INotificationSender
    {
        public async Task NotifyUserUnreadCountAsync(Guid? userId)
        {
            var unreadCount = userId.HasValue
                ? await service.GetUserUnReadNotificationsAsync(userId.Value)
                : 0;
            await hubContext.Clients.Users(userId.ToString()!).SendAsync("NotificationSignal", unreadCount);
        }
    }
}
