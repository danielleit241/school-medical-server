using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class NurseMedicalEventController(IMedicalEventService service) : ControllerBase
    {
        [HttpGet("nurses/students/medical-events")]
        public async Task<IActionResult> GetAllStudentMedicalEvents()
        {
            var medicalEvents = await service.GetAllStudentMedicalEventsAsync();
            if (medicalEvents == null)
            {
                return NotFound(new { Message = "No medical events found." });
            }
            return Ok(medicalEvents);
        }

        [HttpGet("nurse/students/medical-events/{medicalEventId}")]
        public async Task<IActionResult> GetMedicalEventDetail(Guid medicalEventId)
        {
            var medicalEvent = await service.GetMedicalEventDetailAsync(medicalEventId);
            if (medicalEvent == null)
            {
                return NotFound(new { Message = "Medical event not found." });
            }
            return Ok(medicalEvent);
        }

        [HttpPost("nurses/students/medical-events")]
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

            var isCreate = await service.CreateMedicalEventAsync(request);
            if (!isCreate)
            {
                return BadRequest("Failed to create medical event.");
            }
            return StatusCode(201, new { Message = "Medical event created successfully." });
        }
    }
}
