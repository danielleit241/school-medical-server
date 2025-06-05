using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationMedicalEventController(INotificationService service, IHubContext hubContext) : ControllerBase
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
            var unreadCount = await service.GetUserUnReadNotificationsAsync(notification.ReceiverInformationDto.UserId);
            await hubContext.Clients.Users(notification.ReceiverInformationDto.UserId.ToString()!).SendAsync("NotificationSignal", unreadCount);
            return Ok(notification);
        }

        [HttpGet("notification/{notificationId}/medical-events")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalEventNotification([FromBody] NotificationRequest request)
        {
            var notification = await service.GetMedicalEventNotificationAsync(request.NotificationTypeId);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }
            return Ok();
        }

    }
}
