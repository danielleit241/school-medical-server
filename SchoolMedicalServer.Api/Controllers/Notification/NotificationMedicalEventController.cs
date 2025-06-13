using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Helpers;

namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationMedicalEventController(INotificationService service, INotificationSender notificationSender) : ControllerBase
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
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }
    }
}
