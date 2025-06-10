using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Hubs;

namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationMedicalEventController(INotificationService service, IHubContext<NotificationHub> hubContext) : ControllerBase
    {
        [HttpPost("notifications/medical-events/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalEventNotificationToParent([FromBody] NotificationRequest request)
        {
            var notification = await service.SendMedicalEventNotificationToParentAsync(request);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }
            await NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);

            return Ok(notification);
        }
        private async Task NotifyUserUnreadCountAsync(Guid? userId)
        {
            var unreadCount = await service.GetUserUnReadNotificationsAsync(userId);
            await hubContext.Clients.Users(userId.ToString()!).SendAsync("NotificationSignal", unreadCount);
        }
    }
}
