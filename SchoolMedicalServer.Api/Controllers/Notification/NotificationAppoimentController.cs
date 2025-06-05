using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Hubs;

namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationAppoimentController(INotificationService service, IHubContext<NotificationHub> hubContext) : ControllerBase
    {
        [HttpPost("notification/appointments/to-nurse")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> SendAppoimentToNurseNotification([FromBody] NotificationRequest request)
        {
            var notification = await service.SendAppoimentNotificationToNurseAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send appointment notification to nurse.");
            }
            await NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }

        [HttpPost("notification/appoiments/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendAppoimentToParentNotification([FromBody] NotificationRequest request)
        {
            var notification = await service.SendAppoimentNotificationToParentAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send appointment notification to parent.");
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
