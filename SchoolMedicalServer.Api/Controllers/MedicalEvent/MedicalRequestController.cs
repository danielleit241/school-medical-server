using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.MedicalEvent
{
    [Route("api")]
    [ApiController]
    public class MedicalRequestController(IMedicalRequestService service) : ControllerBase
    {
        [HttpGet("medical-requests")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetMedicalRequest([FromQuery] PaginationRequest? pagination)
        {
            var results = await service.GetMedicalRequestsAsync(pagination);
            if (results == null)
            {
                return NotFound(new { Message = "No medical requests found." });
            }
            return Ok(results);
        }

        [HttpGet("medical-requests/{requestId}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetMedicalRequest(Guid requestId)
        {
            var result = await service.GetMedicalRequestAsync(requestId);
            if (result == null)
            {
                return NotFound(new { Message = "No medical requests found." });
            }
            return Ok(result);
        }
    }
}
