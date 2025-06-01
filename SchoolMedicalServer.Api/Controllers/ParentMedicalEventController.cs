using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class ParentMedicalEventController(IMedicalEventService service) : ControllerBase
    {
        [HttpGet("parent/students/{studentId}/medical-events")]
        public async Task<IActionResult> GetMedicalEventsByStudentId(Guid studentId)
        {
            var medicalEvents = await service.GetMedicalEventsByStudentIdAsync(studentId);
            if (medicalEvents == null)
            {
                return NotFound(new { Message = "No medical events found for this student." });
            }
            return Ok(medicalEvents);
        }

        [HttpGet("parent/students/medical-events/{medicalEventId}")]
        public async Task<IActionResult> GetMedicalEventDetail(Guid medicalEventId)
        {
            var medicalEvent = await service.GetMedicalEventDetailAsync(medicalEventId);
            if (medicalEvent == null)
            {
                return NotFound(new { Message = "Medical event not found." });
            }
            return Ok(medicalEvent);
        }
    }
}
