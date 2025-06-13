using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Helpers;

namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationAppoimentController(INotificationService service, INotificationSender notificationSender) : ControllerBase
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
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
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
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }
    }
}
