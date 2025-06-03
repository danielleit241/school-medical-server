using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class NotificationMedicalEventController(INotificationService service) : ControllerBase
    {
        [HttpPost("notifications/medical-events/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalEventNotificationToParent([FromBody] Guid medicalEventId)
        {
            // Logic to send notification to parent about the medical event
            return Ok();
        }

        [HttpGet("notification/{notificationId}/medical-events")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalEventNotification(Guid notificationId)
        {
            // Logic to retrieve the medical event notification by ID
            return Ok();
        }

        [HttpGet("notification/users/{userId}/medical-events")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalEventNotificationsByUser(Guid userId)
        {
            // Logic to retrieve all medical event notifications for a specific user
            return Ok();
        }
    }
}
