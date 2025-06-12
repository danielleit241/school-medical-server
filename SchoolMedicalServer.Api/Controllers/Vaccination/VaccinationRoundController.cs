using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Rounds;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Vaccination
{
    [Route("api")]
    [ApiController]
    public class VaccinationRoundController(IVaccinationRoundService service) : ControllerBase
    {
        [HttpGet("vaccination-rounds/{roundId}/students")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> GetStudentsByVacciantionRoundId([FromQuery] PaginationRequest? pagination, Guid roundId)
        {
            var vaccinationRound = await service.GetStudentsByVacciantionRoundIdAsync(pagination, roundId);
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

        [HttpGet("vaccination-rounds/nurses/{nurseId}")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetVaccinationRoundsByUserId(Guid nurseId, [FromQuery] PaginationRequest? pagination)
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

        [HttpPut("vaccination-rounds/{roundId}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> UpdateVaccinationRound(Guid roundId, [FromBody] VaccinationRoundRequest request)
        {
            if (request == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedRound = await service.UpdateVaccinationRoundAsync(roundId, request);
            if (updatedRound)
            {
                return NotFound(new { Message = "Vaccination round not found." });
            }
            return Ok(updatedRound);
        }
    }
}
