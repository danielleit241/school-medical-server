using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.MedicalEvent
{
    [Route("api")]
    [ApiController]
    public class ParentMedicalEventController(IMedicalEventService service) : ControllerBase
    {
        [HttpGet("parents/students/{studentId}/medical-events")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalEventsByStudentId([FromQuery] PaginationRequest? paginationRequest, Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                return BadRequest("Student ID cannot be empty.");
            }
            var medicalEvents = await service.GetMedicalEventsByStudentIdAsync(paginationRequest, studentId);
            if (medicalEvents == null)
            {
                return NotFound(new { Message = "No medical events found for this student." });
            }
            return Ok(medicalEvents);
        }

        [HttpGet("parents/students/medical-events/{medicalEventId}")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalEventDetail(Guid medicalEventId)
        {
            if (medicalEventId == Guid.Empty)
            {
                return BadRequest("Medical event ID cannot be empty.");
            }
            var medicalEvent = await service.GetMedicalEventDetailAsync(medicalEventId);
            if (medicalEvent == null)
            {
                return NotFound(new { Message = "Medical event not found." });
            }
            return Ok(medicalEvent);
        }
    }
}
