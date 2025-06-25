namespace SchoolMedicalServer.Api.Controllers.Notification
{
    [Route("api")]
    [ApiController]
    public class NotificationAppoimentController(IAppointmentNotificationService service, INotificationSender notificationSender) : ControllerBase
    {
        [HttpPost("notification/appointments/to-nurse")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> SendAppoimentToNurseNotification([FromBody] NotificationRequest request)
        {
            var notification = await service.SendAppointmentNotificationToNurseAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send appointment notification to nurse.");
            }
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }

        [HttpPost("notification/appoiments/to-parent")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> SendAppoimentToParentNotification([FromBody] NotificationRequest request)
        {
            var notification = await service.SendAppointmentNotificationToParentAsync(request);
            if (notification == null)
            {
                return BadRequest("Failed to send appointment notification to parent.");
            }
            await notificationSender.NotifyUserUnreadCountAsync(notification.ReceiverInformationDto.UserId);
            return Ok(notification);
        }
    }
}
