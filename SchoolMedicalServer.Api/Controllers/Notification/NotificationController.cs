using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Api.Hubs;

namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationController(INotificationService service) : ControllerBase
    {
        [HttpGet]
        [Route("users/{userId}/notifications")]
        [Authorize]
        public async Task<IActionResult> GetNotificationsHistory([FromQuery] PaginationRequest? request, Guid userId)
        {
            var notis = await service.GetUserNotificationsAsync(request, userId);
            if (notis == null)
            {
                return NotFound(new { Message = "No notifications found." });
            }
            return Ok(notis);
        }

        [HttpGet]
        [Route("users/{userId}/notifications/unread")]
        [Authorize]
        public async Task<IActionResult> GetUnReadNotifications(Guid userId)
        {
            var count = await service.GetUserUnReadNotificationsAsync(userId);
            return Ok(new { unreadCount = count });
        }

        [HttpPut]
        [Route("users/{userId}/notifications")]
        [Authorize]
        public async Task<IActionResult> ReadNotifications(Guid userId)
        {
            var isReaded = await service.ReadAllNotificationsAsync(userId);
            if (!isReaded)
            {
                return NotFound(new { Message = "No notifications found to mark as read." });
            }
            return Ok(new { Message = "Readed" });
        }
    }
}
