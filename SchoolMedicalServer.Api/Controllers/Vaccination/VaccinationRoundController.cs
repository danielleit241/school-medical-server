namespace SchoolMedicalServer.Api.Controllers.Vaccination
{
    [Route("api")]
    [ApiController]
    public class VaccinationRoundController(IVaccinationRoundService service) : ControllerBase
    {
        [HttpPost("schedules/vaccination-rounds")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> CreateVaccinationRound([FromBody] VaccinationRoundRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { Message = "Invalid request data." });
            }
            var result = await service.CreateVaccinationRoundByScheduleIdAsync(request);
            if (!result)
            {
                return NotFound(new { Message = "Vaccination round not found or update failed." });
            }
            return Ok(new { Message = "Vaccination round updated successfully." });
        }

        [HttpGet("managers/vaccination-rounds/{roundId}/students")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetStudentsByVacciantionRoundId([FromQuery] PaginationRequest? pagination, Guid roundId)
        {
            var vaccinationRound = await service.GetStudentsByVacciantionRoundIdForManagerAsync(pagination, roundId);
            if (vaccinationRound == null)
            {
                return NotFound(new { Message = "Vaccination round not found." });
            }
            return Ok(vaccinationRound);
        }

        [HttpGet("vaccination-rounds/{roundId}")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> GetVaccinationRoundById(Guid roundId)
        {
            var vaccinationRound = await service.GetVaccinationRoundByIdAsync(roundId);
            if (vaccinationRound == null)
            {
                return NotFound(new { Message = "Vaccination round not found." });
            }
            return Ok(vaccinationRound);
        }

        [HttpGet("nurses/{nurseId}/vaccination-rounds/{roundId}/students")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetStudentsByVacciantionRoundIdForNurse([FromQuery] PaginationRequest? pagination, Guid roundId, Guid nurseId)
        {
            var vaccinationRound = await service.GetStudentsByVacciantionRoundIdForNurseAsync(pagination, roundId, nurseId);
            if (vaccinationRound == null)
            {
                return NotFound(new { Message = "Vaccination round not found." });
            }
            return Ok(vaccinationRound);
        }


        [HttpGet("v2/nurses/{nurseId}/vaccination-rounds/{roundId}/students")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetStudentsByRoundIdForNurse(Guid roundId, Guid nurseId)
        {
            var vaccinationRound = await service.GetStudentsByVacciantionRoundIdForNurseAsync(roundId, nurseId);
            if (vaccinationRound == null)
            {
                return NotFound(new { Message = "Vaccination round not found." });
            }
            return Ok(vaccinationRound);
        }

        [HttpGet("nurses/{nurseId}/vaccination-rounds")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetVaccinationRoundsByNurseId(Guid nurseId, [FromQuery] PaginationRequest? pagination)
        {
            var vaccinationRounds = await service.GetVaccinationRoundsByNurseIdAsync(nurseId, pagination);
            if (vaccinationRounds == null || !vaccinationRounds.Items.Any())
            {
                return NotFound(new { Message = "No vaccination rounds found for this user." });
            }
            return Ok(vaccinationRounds);
        }

        [HttpGet("schedules/{scheduleId}/vaccination-rounds")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetVaccinationRounds(Guid scheduleId)
        {
            var vaccinationRounds = await service.GetVaccinationRoundsByScheduleIdAsync(scheduleId);
            if (vaccinationRounds == null)
            {
                return NotFound(new { Message = "No vaccination rounds found." });
            }
            return Ok(vaccinationRounds);
        }

        [HttpPut("vaccination-rounds/{roundId}/finished")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> UpdateVaccinationRoundStatus(Guid roundId, [FromBody] bool request)
        {
            if (roundId == Guid.Empty)
            {
                return BadRequest(new { Message = "Invalid round ID." });
            }
            var result = await service.UpdateVaccinationRoundStatusAsync(roundId, request);
            if (!result)
            {
                return NotFound(new { Message = "Vaccination round not found or update failed." });
            }
            return Ok(new { Message = "Vaccination round status updated successfully." });
        }

        [HttpPut("vaccination-rounds/{roundId}")]
        public async Task<IActionResult> UpdateVaccinationRound(Guid roundId, [FromBody] VaccinationRoundUpdateRequest request)
        {
            if (roundId == Guid.Empty || request == null)
            {
                return BadRequest(new { Message = "Invalid request data." });
            }
            var result = await service.UpdateVaccinationRoundAsync(roundId, request);
            if (!result)
            {
                return NotFound(new { Message = "Vaccination round not found or update failed." });
            }
            return Ok(new { Message = "Vaccination round updated successfully." });
        }

        [HttpGet("parents/{userId}/vaccination-rounds/students")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetStudentRoundByUserId(Guid userId, [FromQuery] DateOnly? start, [FromQuery] DateOnly? end)
        {
            var vaccinationRounds = await service.GetVaccinationRoundsByUserIdAsync(userId, start, end);
            if (vaccinationRounds == null)
            {
                return NotFound(new { Message = "No vaccination rounds found for this user." });
            }
            return Ok(vaccinationRounds);
        }
    }
}
