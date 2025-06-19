using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Results;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.HealthCheck
{
    [Route("api")]
    [ApiController]
    public class HealthCheckResultController(IHealthCheckResultService service) : ControllerBase
    {
        [HttpGet("health-check-results/{resultId}/is-confirmed")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> IsHealthCheckConfirmed(Guid resultId)
        {
            var result = await service.IsHealthCheckConfirmed(resultId);
            if (result == null)
            {
                return NotFound(new { Message = "Parent is not confirm or decline" });
            }
            return Ok(result);
        }

        [HttpPut("health-check-results/{resultId}/confirm")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> ConfirmOrDeclineHealthCheck(Guid resultId, [FromBody] ParentHealthCheckConfirmationRequest request)
        {
            var result = await service.ConfirmOrDeclineHealthCheck(resultId, request);
            if (result == null)
            {
                return NotFound(new { Message = "Health check round not found." });
            }
            if (result == false)
            {
                return Ok(new { Message = "Parent is decline" });
            }
            return Ok(new { Message = "Parent is confirm" });
        }

        [HttpPost("health-check-results")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> CreateHealthCheckResult([FromBody] HealthCheckResultRequest request)
        {
            var notification = await service.CreateHealthCheckResultAsync(request);
            if (notification == null)
            {
                return BadRequest(new { Message = "Failed to create health check result." });
            }
            return Ok(notification);
        }

        [HttpGet("health-check-results/{resultId}")]
        [Authorize(Roles = "admin, manager, nurse, parent")]
        public async Task<IActionResult> GetHealthCheckResult(Guid resultId)
        {
            var result = await service.GetHealthCheckResultAsync(resultId);
            if (result == null)
            {
                return NotFound(new { Message = "Health check result not found." });
            }
            return Ok(result);
        }

        [HttpGet("health-check-results/students/{studentId}")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetHealthCheckResultsByStudentId([FromQuery] PaginationRequest? request, Guid studentId)
        {
            var results = await service.GetHealthCheckResultsByStudentIdAsync(request, studentId);
            if (results == null)
            {
                return NotFound(new { Message = "No health check results found for this student." });
            }
            return Ok(results);
        }
    }
}
