using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class NotificationMedicalRegistrationController(INotificationService service) : ControllerBase
    {
        [HttpPost("notifications/medical-registrations/approved/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalRegistrationNotificationToParent([FromBody] NotificationRequest request, [FromQuery] Guid medicalRegistrationId)
        {
            var notification = await service.SendMedicalRegistrationNotificationToParentAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send medical registration notification to parent.");
            }
            return Ok(notification);
        }

        [HttpPost("notifications/medical-registrations/completed/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalRegistrationDetailsNotificationToParent([FromBody] NotificationRequest request, [FromQuery] Guid medicalRegistrationId)
        {
            var notification = await service.SendMedicalRegistrationDetailsNotificationToParentAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send medical registration details notification to parent.");
            }
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

        [HttpGet("notifications/users/{userId}/medical-registrations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalRegistrationNotifications([FromQuery] PaginationRequest pagination, Guid userId)
        {
            var notifications = await service.GetMedicalRegistrationNotificationsAsync(pagination, userId);
            return Ok(notifications);
        }
    }
}
