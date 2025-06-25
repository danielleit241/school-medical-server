namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationHealthCheckController(IHealthCheckNotificationService service, INotificationSender notificationSender) : ControllerBase
    {
        [HttpPost("notifications/health-checks/to-parent")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> SendHealthCheckNotificationToParent([FromBody] IEnumerable<NotificationRequest> requests)
        {
            var notifications = await service.SendHealthCheckNotificationToParents(requests);
            if (notifications == null)
            {
                return BadRequest("Failed to send health-check notification to parent.");
            }
            foreach (var notification in notifications)
            {
                await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            }
            return Ok(notifications);
        }

        [HttpPost("notifications/health-checks/results/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendHealthCheckResultNotificationToParent([FromBody] NotificationRequest request)
        {
            var notification = await service.SendHealthCheckResultNotificationToParent(request);
            if (notification == null)
            {
                return BadRequest("Failed to send health-check result notification to parent.");
            }
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }


        [HttpPost("notifications/health-checks/to-nurse")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> SendHealthCheckNotificationToNurse([FromBody] IEnumerable<NotificationRequest> requests)
        {
            var notifications = await service.SendHealthCheckNotificationToNurses(requests);
            if (notifications == null)
            {
                return BadRequest("Failed to send health-check notification to nurse.");
            }
            foreach (var notification in notifications)
            {
                await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            }
            return Ok(notifications);
        }
    }
}
