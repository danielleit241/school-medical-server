namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationHealthCheckController(IHealthCheckNotificationService service, INotificationSender notificationSender, IEmailHelper emailHelper, IWebHostEnvironment env, IUserService userService) : ControllerBase
    {
        [HttpPost("notifications/health-checks/rounds/to-admin")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendHealthCheckNotificationToAdmin([FromBody] NotificationRequest request)
        {
            var notification = await service.SendHealthCheckNotificationToAdmin(request);
            if (notification == null)
            {
                return BadRequest("Failed to send health-check notification to admin.");
            }
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }

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
                var parent = await userService.GetUserAsync(notification.ReceiverInformationDto.UserId!.Value);
                await SendEmailToParent(parent?.EmailAddress ?? string.Empty, parent?.FullName ?? "Parent");
            }
            return Ok(notifications);
        }

        private async Task SendEmailToParent(string parentEmail, string parentName)
        {
            string templateName = "email_schedule_reminders.html";
            string templatePath = Path.Combine(env.WebRootPath, "templates", templateName);
            string emailTemplate = System.IO.File.ReadAllText(templatePath);
            string html = emailTemplate
                .Replace("{ParentName}", parentName ?? "Parent")
                .Replace("{Type}", "Vaccination");
            await emailHelper.SendEmailAsync(new EmailFrom
            {
                To = parentEmail,
                Subject = "[MEDICARE] Please confirm Vaccination result",
                Body = html
            });
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
