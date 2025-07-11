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
                return NotFound(new { Message = "Health check round not found or update failed." });
            }
            return Ok(new { Message = "Health check round updated successfully." });
        }

        [HttpGet("schedules/{scheduleId}/health-check-rounds/supplementary/total-students")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetTotalSupplementaryTotalStudents(Guid scheduleId)
        {
            if (scheduleId == Guid.Empty)
            {
                return BadRequest(new { Message = "Invalid request data." });
            }
            var result = await service.GetTotalSupplementaryTotalStudentsAsync(scheduleId);
            return Ok(new { SupplementStudents = result });
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

        [HttpGet("v2/nurses/{nurseId}/health-check-rounds/{roundId}/students")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetStudentsByHealthCheckRoundIdForNurse(Guid roundId, Guid nurseId)
        {
            var students = await service.GetStudentsByHealthCheckRoundIdForNurseAsync(roundId, nurseId);
            if (students == null)
            {
                return NotFound(new { Message = "Health check round not found." });
            }
            return Ok(students);
        }

        [HttpGet("nurses/{nurseId}/health-check-rounds")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetHealthCheckRoundsByNurseId(Guid nurseId, [FromQuery] PaginationRequest? pagination)
        {
            var vaccinationRounds = await service.GetHealthCheckRoundsByNurseIdAsync(nurseId, pagination);
            if (vaccinationRounds == null || !vaccinationRounds.Items.Any())
            {
                return NotFound(new { Message = "No Health check rounds found for this user." });
            }
            return Ok(vaccinationRounds);
        }


        [HttpPut("health-check-rounds/{roundId}/finished")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> UpdateHealthCheckRoundStatus(Guid roundId, [FromBody] bool request)
        {
            if (roundId == Guid.Empty)
            {
                return BadRequest(new { Message = "Invalid round ID." });
            }
            var result = await service.UpdateHealthCheckRoundStatusAsync(roundId, request);
            if (result == null)
            {
                return NotFound(new { Message = "Health check round not found or update failed." });
            }
            return Ok(result);
        }

        [HttpPut("health-check-rounds/{roundId}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> UpdateHealthCheckRound(Guid roundId, [FromBody] HealthCheckRoundUpdateRequest request)
        {
            if (roundId == Guid.Empty || request == null)
            {
                return BadRequest(new { Message = "Invalid request data." });
            }
            var result = await service.UpdateHealthCheckRoundAsync(roundId, request);
            if (!result)
            {
                return NotFound(new { Message = "Health check round not found or update failed." });
            }
            return Ok(new { Message = "Health check round updated successfully." });
        }


        [HttpGet("parents/{userId}/health-check-rounds/students")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetStudentRoundByUserId(Guid userId, [FromQuery] DateOnly? start, [FromQuery] DateOnly? end)
        {
            var healthCheckRounds = await service.GetHealthCheckRoundsByUserIdAsync(userId, start, end);
            if (healthCheckRounds == null)
            {
                return NotFound(new { Message = "No Health check rounds found for this user." });
            }
            return Ok(healthCheckRounds);
        }

        [HttpGet("health-check-rounds/{roundId}")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> GetHealthCheckRoundById(Guid roundId)
        {
            var healthCheckRound = await service.GetHealthCheckRoundByIdAsync(roundId);
            if (healthCheckRound == null)
            {
                return NotFound(new { Message = "Health check round not found." });
            }
            return Ok(healthCheckRound);
        }
    }
}
