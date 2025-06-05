using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationMedicalRegistrationController(INotificationService service, IHubContext hubContext) : ControllerBase
    {
        [HttpPost("notifications/medical-registrations/approved/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalRegistrationNotificationToParent([FromBody] NotificationRequest request)
        {
            var notification = await service.SendMedicalRegistrationApprovedNotificationToParentAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send medical registration notification to parent.");
            }
            var unreadCount = await service.GetUserUnReadNotificationsAsync(notification.ReceiverInformationDto.UserId);
            await hubContext.Clients.Users(notification.ReceiverInformationDto.UserId.ToString()!).SendAsync("NotificationSignal", unreadCount);
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
            var unreadCount = await service.GetUserUnReadNotificationsAsync(notification.ReceiverInformationDto.UserId);
            await hubContext.Clients.Users(notification.ReceiverInformationDto.UserId.ToString()!).SendAsync("NotificationSignal", unreadCount);
            return Ok(notification);
        }

        [HttpGet("notifications/{notificationId}/medical-registrations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalRegistrationNotification(Guid notificationId)
        {
            var notification = await service.GetMedicalRegistrationNotificationAsync(notificationId);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }
            return Ok(notification);
        }

        //[HttpGet("notifications/users/{userId}/medical-registrations")]
        //[Authorize(Roles = "parent")]
        //public async Task<IActionResult> GetMedicalRegistrationNotifications([FromQuery] PaginationRequest pagination, Guid userId)
        //{
        //    var notifications = await service.GetMedicalRegistrationNotificationsAsync(pagination, userId);
        //    return Ok(notifications);
        //}
    }
}
