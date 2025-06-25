namespace SchoolMedicalServer.Api.Controllers.MedicalEvent
{
    [Route("api")]
    [ApiController]
    public class NurseMedicalEventController(IMedicalEventService service) : ControllerBase
    {
        [HttpGet("nurses/students/medical-events")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetAllStudentMedicalEvents([FromQuery] PaginationRequest? paginationRequest)
        {
            var medicalEvents = await service.GetAllStudentMedicalEventsAsync(paginationRequest);
            if (medicalEvents == null)
            {
                return NotFound(new { Message = "No medical events found." });
            }
            return Ok(medicalEvents);
        }

        [HttpGet("nurses/students/medical-events/{medicalEventId}")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetMedicalEventDetail(Guid medicalEventId)
        {
            if (medicalEventId == Guid.Empty)
            {
                return BadRequest(new { Message = "Medical event ID cannot be empty." });
            }
            var medicalEvent = await service.GetMedicalEventDetailAsync(medicalEventId);
            if (medicalEvent == null)
            {
                return NotFound(new { Message = "Medical event not found." });
            }
            return Ok(medicalEvent);
        }

        [HttpPost("nurses/students/medical-events")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> CreateStudentMedicalEvent([FromBody] MedicalEventRequest request)
        {
            if (request.MedicalRequests != null)
            {
                var isEnoughQuantity = await service.IsEnoughQuantityAsync(request.MedicalRequests);
                if (!isEnoughQuantity)
                {
                    return BadRequest("Not enough quantity for the requested items.");
                }
            }

            var medicalEvent = await service.CreateMedicalEventAsync(request);
            if (medicalEvent == null)
            {
                return BadRequest("Failed to create medical event.");
            }
            return StatusCode(201, medicalEvent);
        }
    }
}
