using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationController(INotificationService service) : ControllerBase
    {
        [HttpGet]
        [Route("users/{userId}/notifications")]
        [Authorize]
        public async Task<IActionResult> GetNotifications(PaginationRequest? request, Guid userId)
        {
            var notis = await service.GetUserNotificationsAsync(request, userId);
            if (notis == null)
            {
                return NotFound(new { Message = "No notifications found." });
            }
            return Ok(notis);
        }
    }
}
