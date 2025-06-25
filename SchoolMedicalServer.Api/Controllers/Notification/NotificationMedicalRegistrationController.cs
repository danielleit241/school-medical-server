namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationMedicalRegistrationController(IMedicalRegistrationNotificationService service, INotificationSender notificationSender) : ControllerBase
    {
        [HttpPost("notifications/medical-registrations/to-nurse")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> SendMedicalRegistrationNotificationToNurse([FromBody] NotificationRequest request)
        {
            var notification = await service.SendMedicalRegistrationNotificationToNurseAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send medical registration notification to parent.");
            }
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }

        [HttpPost("notifications/medical-registrations/approved/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalRegistrationNotificationToParent([FromBody] NotificationRequest request)
        {
            var notification = await service.SendMedicalRegistrationApprovedNotificationToParentAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send medical registration notification to parent.");
            }
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }

        [HttpPost("notifications/medical-registrations/completed/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendMedicalRegistrationDetailsNotificationToParent([FromBody] NotificationRequest request)
        {
            var notification = await service.SendMedicalRegistrationCompletedNotificationToParentAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send medical registration details notification to parent.");
            }
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }
    }
}
