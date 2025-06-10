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
    public class NotificationMedicalRegistrationController(INotificationService service, IHubContext<NotificationHub> hubContext) : ControllerBase
    {
        [HttpPost("notifications/medical-registrations/to-nurse")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> SendMedicalRegistrationNotificationToNurse([FromBody] NotificationRequest request)
        {
            var notification = await service.SendMedicalRegistrationNotificationToNurseAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send medical registration notification to parent.");
            }
            await NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }

        [HttpPost("notifications/medical-registrations/approved/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalRegistrationNotificationToParent([FromBody] NotificationRequest request)
        {
            var notification = await service.SendMedicalRegistrationApprovedNotificationToParentAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send medical registration notification to parent.");
            }
            await NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }

        [HttpPost("notifications/medical-registrations/completed/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalRegistrationDetailsNotificationToParent([FromBody] NotificationRequest request)
        {
            var notification = await service.SendMedicalRegistrationCompletedNotificationToParentAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send medical registration details notification to parent.");
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
