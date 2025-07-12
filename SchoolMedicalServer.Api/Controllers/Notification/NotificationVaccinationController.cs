namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationVaccinationController(IVaccinationNotificationService service, INotificationSender notificationSender, IEmailHelper emailHelper, IWebHostEnvironment env, IUserService userService) : ControllerBase
    {
        [HttpPost("notifications/vaccinations/rounds/to-admin")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendVaccinationNotificationToAdmin([FromBody] NotificationRequest request)
        {
            var notification = await service.SendVaccinationNotificationToAdmin(request);
            if (notification == null)
            {
                return BadRequest("Failed to send vaccination notification to admin.");
            }

            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }

        [HttpPost("notifications/vaccinations/to-parent")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> SendVaccinationNotificationToParent([FromBody] IEnumerable<NotificationRequest> requests)
        {
            var notifications = await service.SendVaccinationResultNotificationToParents(requests);
            if (notifications == null)
            {
                return BadRequest("Failed to send vaccination notification to parent.");
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
                .Replace("{ParentName}", parentName)
                .Replace("{Type}", "Health Check");
            await emailHelper.SendEmailAsync(new EmailFrom
            {
                To = parentEmail,
                Subject = "[MEDICARE] Please confirm Health Check result",
                Body = html
            });
        }

        [HttpPost("notifications/vaccinations/results/to-parent")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> SendVaccinationResultNotificationToParent([FromBody] NotificationRequest request)
        {
            var notification = await service.SendVaccinationResultNotificationToParent(request);
            if (notification == null)
            {
                return BadRequest("Failed to send vaccination notification to parent.");
            }
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
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
