using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class NotificationAppoimentController(INotificationService service) : ControllerBase
    {
        [HttpPost("notification/appoiments/to-nurse")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> SendAppoimentToNurseNotification([FromBody] NotificationRequest request)
        {
            var notification = await service.SendAppoimentToNurseNotificationAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send appointment notification to nurse.");
            }
            return Ok(notification);
        }

        [HttpPost("notification/appoiments/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendAppoimentToParentNotification([FromBody] NotificationRequest request)
        {
            var notification = await service.SendAppoimentToParentNotificationAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send appointment notification to parent.");
            }
            return Ok(notification);
        }

        [HttpGet("notification/{notificationId}/appoiments")]
        [Authorize(Roles = "parent, nurse")]
        public async Task<IActionResult> GetAppoimentNotification(Guid notificationId)
        {
            var notification = await service.GetAppoimentNotificationAsync(notificationId);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }
            return Ok(notification);
        }

        [HttpGet("notification/users/{userId}/appoiments")]
        [Authorize(Roles = "parent, nurse")]
        public async Task<IActionResult> GetAppoimentNotificationsByUser([FromQuery] PaginationRequest pagination, Guid userId)
        {
            var notifications = await service.GetAppoimentNotificationsByUserAsync(pagination, userId);
            if (notifications == null)
            {
                return NotFound("No notifications found for the specified user.");
            }
            return Ok();
        }
    }
}
