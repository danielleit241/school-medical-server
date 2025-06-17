using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Rounds;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.HealthCheck
{
    [Route("api")]
    [ApiController]
    public class HealthCheckRoundController(IHealthCheckRoundService service) : ControllerBase
    {
        [HttpPost("schedules/health-check-rounds")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> CreateHealthCheckRound([FromBody] HealthCheckRoundRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { Message = "Invalid request data." });
            }
            var result = await service.CreateHealthCheckRoundByScheduleIdAsync(request);
            if (!result)
            {
                return NotFound(new { Message = "Vaccination round not found or update failed." });
            }
            return Ok(new { Message = "Vaccination round updated successfully." });
        }

        [HttpGet("managers/health-check-rounds/{roundId}/students")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetStudentsByHealthCheckRoundId([FromQuery] PaginationRequest? pagination, Guid roundId)
        {
            var students = await service.GetStudentsByHealthCheckRoundIdForManagerAsync(pagination, roundId);
            if (students == null)
            {
                return NotFound(new { Message = "Health check round not found." });
            }
            return Ok(students);
        }

        [HttpGet("nurses/{nurseId}/health-check-rounds/{roundId}/students")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetStudentsByHealthCheckRoundIdForNurse([FromQuery] PaginationRequest? pagination, Guid roundId, Guid nurseId)
        {
            var students = await service.GetStudentsByHealthCheckRoundIdForNurseAsync(pagination, roundId, nurseId);
            if (students == null)
            {
                return NotFound(new { Message = "Health check round not found." });
            }
            return Ok(students);
        }

        [HttpGet("nurses/{nurseId}/health-check-rounds")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetVaccinationRoundsByUserId(Guid nurseId, [FromQuery] PaginationRequest? pagination)
        {
            var vaccinationRounds = await service.GetHealthCheckRoundsByNurseIdAsync(nurseId, pagination);
            if (vaccinationRounds == null || !vaccinationRounds.Items.Any())
            {
                return NotFound(new { Message = "No vaccination rounds found for this user." });
            }
            return Ok(vaccinationRounds);
        }
    }
}
