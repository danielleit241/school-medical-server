using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class ParentMedicalEventController(IMedicalEventService service) : ControllerBase
    {
        [HttpGet("parents/students/{studentId}/medical-events")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalEventsByStudentId([FromQuery] PaginationRequest? paginationRequest, Guid studentId)
        {
            var medicalEvents = await service.GetMedicalEventsByStudentIdAsync(paginationRequest, studentId);
            if (medicalEvents == null)
            {
                return NotFound(new { Message = "No medical events found for this student." });
            }
            return Ok(medicalEvents);
        }

        [HttpGet("parents/students/medical-events/{medicalEventId}")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalEventDetail([FromQuery] PaginationRequest? paginationRequest, Guid medicalEventId)
        {
            var medicalEvent = await service.GetMedicalEventDetailAsync(paginationRequest, medicalEventId);
            if (medicalEvent == null)
            {
                return NotFound(new { Message = "Medical event not found." });
            }
            return Ok(medicalEvent);
        }
    }
}
