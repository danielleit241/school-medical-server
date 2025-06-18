using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Results;
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
            var result = await service.CreateHealthCheckResultAsync(request);
            if (!result)
            {
                return BadRequest(new { Message = "Failed to create health check result." });
            }
            return Ok();
        }

        [HttpGet("health-check-results/{resultId}")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> GetHealthCheckResult(Guid resultId)
        {
            var result = await service.GetHealthCheckResultAsync(resultId);
            if (result == null)
            {
                return NotFound(new { Message = "Health check result not found." });
            }
            return Ok(result);
        }
    }
}
