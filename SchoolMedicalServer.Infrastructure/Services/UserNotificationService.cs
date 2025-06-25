using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class UserNotificationService(
        INotificationRepository notificationRepository,
        INotificationHelperService helperService) : IUserNotificationService
    {

        public async Task<PaginationResponse<NotificationResponse>> GetUserNotificationsAsync(PaginationRequest? pagination, Guid userId)
        {
            var totalCount = await notificationRepository.CountByUserIdAsync(userId);

            if (totalCount == 0)
            {
                return null!;
            }

            var skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var notifications = await notificationRepository.GetByUserIdPagedAsync(userId, skip, pagination?.PageSize ?? 10);

            var result = new List<NotificationResponse>();

            foreach (var notification in notifications)
            {
                var request = new NotificationRequest
                {
                    SenderId = notification.SenderId,
                    ReceiverId = notification.UserId
                };
                var notiInfo = helperService.GetNotificationInformation(notification);
                var sender = await helperService.GetSenderInformationAsync(request)!;
                var receiver = await helperService.GetReceiverInformationAsync(request)!;

                result.Add(helperService.GetNotificationResponse(notiInfo, sender, receiver));
            }

            return new PaginationResponse<NotificationResponse>(
                   pagination!.PageIndex,
                   pagination.PageSize,
                   totalCount,
                   result
            );
        }

        public async Task<bool> ReadAllNotificationsAsync(Guid userId)
        {
            var notifications = await notificationRepository.GetUnreadByUserIdAsync(userId);
            if (notifications.Count == 0)
            {
                return false;
            }
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadDate = DateTime.UtcNow;
            }
            await notificationRepository.UpdateRangeAsync(notifications);
            return true;
        }

        public async Task<int> GetUserUnReadNotificationsAsync(Guid? userId)
        {
            var unreadNotis = await notificationRepository.CountUnreadByUserIdAsync(userId ?? Guid.Empty);
            return unreadNotis;
        }

        public async Task<NotificationResponse> GetUserNotificationDetailsAsync(Guid notificationId)
        {
            var noti = await notificationRepository.GetByIdAsync(notificationId);
            if (noti == null)
            {
                return null!;
            }
            var request = new NotificationRequest
            {
                SenderId = noti.SenderId,
                ReceiverId = noti.UserId
            };
            var notiInfo = helperService.GetNotificationInformation(noti);
            var sender = await helperService.GetSenderInformationAsync(request);
            var receiver = await helperService.GetReceiverInformationAsync(request);
            return helperService.GetNotificationResponse(notiInfo, sender, receiver);
        }
    }
}
