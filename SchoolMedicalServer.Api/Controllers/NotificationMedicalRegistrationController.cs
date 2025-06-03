using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class NotificationMedicalRegistrationController(INotificationService service) : ControllerBase
    {
        [HttpPost("notifications/medical-registrations/approved/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalRegistrationNotificationToParent([FromBody] Guid medicalRegistrationId)
        {
            return Ok();
        }

        [HttpPost("notifications/medical-registrations/completed/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalRegistrationDetailsNotificationToParent([FromBody] Guid medicalRegistrationId)
        {
            return Ok();
        }

        [HttpGet("notifications/{notificationId}/medical-registrations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalRegistrationNotification(Guid notificationId)
        {
            return Ok();
        }

        [HttpGet("notifications/users/{userId}/medical-registrations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalRegistrationNotifications(Guid userId)
        {
            // Implement pagination logic here
            return Ok();

        }
    }
}
