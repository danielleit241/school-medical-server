namespace SchoolMedicalServer.Api.Controllers.MedicalEvent
{
    [Route("api")]
    [ApiController]
    public class MedicalEventController(IMedicalEventService service) : ControllerBase
    {
        [HttpGet("medical-events/{eventId}")]
        [Authorize(Roles = ("admin, manager"))]
        public async Task<IActionResult> GetMedicalEvent(Guid eventId)
        {
            var medicalEvent = await service.GetMedicalEventDetailAsync(eventId);
            if (medicalEvent is null)
            {
                return NotFound();
            }
            return Ok(medicalEvent);
        }
    }
}
