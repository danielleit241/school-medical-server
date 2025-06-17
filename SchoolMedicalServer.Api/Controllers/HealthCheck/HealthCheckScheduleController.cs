using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Schedules;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.HealthCheck
{
    [Route("api")]
    [ApiController]
    public class HealthCheckScheduleController(IHealthCheckService service) : ControllerBase
    {
        [HttpPost("health-checks/schedules")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> CreateHealthCheckScheduleSchedule([FromBody] HealthCheckScheduleRequest request)
        {
            var isCreated = await service.CreateScheduleAsync(request);
            if (!isCreated)
            {
                return BadRequest("Create schedule failed");
            }
            return Ok("Create schedule successfully");
        }

        [HttpPost("health-checks/schedules/add-students")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> AddStudentInRoundByScheduleId([FromBody] Guid scheduleId)
        {
            var notificationRequests = await service.CreateVaccinationResultsByRounds(scheduleId);
            if (notificationRequests == null)
            {
                return BadRequest(new { Message = "Failed to create vaccination schedule." });
            }
            return Ok(notificationRequests);
        }

        [HttpGet("health-checks/schedules")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetVaccinationSchedules([FromQuery] PaginationRequest? pagination)
        {
            var healthCheckSchedules = await service.GetPaginationHealthCheckSchedule(pagination);
            if (healthCheckSchedules == null)
            {
                return NotFound(new { Message = "No healthcheck schedules found." });
            }
            return Ok(healthCheckSchedules);
        }

        [HttpGet("health-checks/schedules/{id}")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> GetVaccinationSchedule(Guid id)
        {
            var healthCheckSchedule = await service.GetHealthCheckSchedule(id);
            if (healthCheckSchedule == null)
            {
                return NotFound(new { Message = "No healthcheck schedules found." });
            }
            return Ok(healthCheckSchedule);
        }

        [HttpPut("health-check/schedules/{scheduleId}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> UpdateHealthCheckSchedule(Guid scheduleId, [FromBody] HealthCheckScheduleUpdateRequest request)
        {
            var isUpdated = await service.UpdateScheduleAsync(scheduleId, request);
            if (!isUpdated)
            {
                return BadRequest(new { Message = "Failed to update healthcheck schedule." });
            }
            return Ok("Update schedule successfully!");
        }
    }
}
