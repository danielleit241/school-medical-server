using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Helpers;

namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationVaccinationController(INotificationService service, INotificationSender notificationSender) : ControllerBase
    {
        [HttpPost("notifications/vaccinations/to-parent")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> SendVaccinationNotificationToParent([FromBody] IEnumerable<NotificationRequest> requests)
        {
            var notifications = await service.SendVaccinationNotificationToParents(requests);
            if (notifications == null)
            {
                return BadRequest("Failed to send vaccination notification to parent.");
            }
            foreach (var notification in notifications)
            {
                await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            }
            return Ok(notifications);
        }

        [HttpPost("notifications/vaccinations/observations/to-parent")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> SendVaccinationObservationNotificationToParent([FromBody] NotificationRequest requests)
        {
            var notification = await service.SendVaccinationObservationNotificationToParent(requests);
            if (notification == null)
            {
                return BadRequest("Failed to send vaccination observation notification.");
            }
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }

        [HttpPost("notifications/vaccinations/to-nurse")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> SendVaccinationNotificationToNurse([FromBody] IEnumerable<NotificationRequest> requests)
        {
            var notifications = await service.SendVaccinationNotificationToNurses(requests);
            if (notifications == null)
            {
                return BadRequest("Failed to send vaccination notification to nurse.");
            }
            foreach (var notification in notifications)
            {
                await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            }
            return Ok(notifications);
        }
    }
}
