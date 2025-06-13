using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Vaccination
{
    [Route("api")]
    [ApiController]
    public class VaccinationScheduleController(IVaccinationScheduleService service) : ControllerBase
    {
        [HttpPost("vaccinations/schedules")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> CreateVaccinationSchedule([FromBody] VaccinationScheduleRequest request)
        {
            var isCreated = await service.CreateScheduleAsync(request);
            if (!isCreated)
            {
                return BadRequest("Create schedule failed");
            }
            return Ok("Create schedule successfully");
        }

        [HttpPost("vaccination/schedules/add-students")]
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

        [HttpGet("vaccinations/schedules")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetVaccinationSchedules([FromQuery] PaginationRequest? pagination)
        {
            var vaccinationSchedules = await service.GetPaginationVaccinationSchedule(pagination);
            if (vaccinationSchedules == null)
            {
                return NotFound(new { Message = "No vaccination schedules found." });
            }
            return Ok(vaccinationSchedules);
        }

        [HttpGet("vaccinations/schedules/{id}")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> GetVaccinationSchedule(Guid id)
        {
            var vaccinationSchedule = await service.GetVaccinationSchedule(id);
            if (vaccinationSchedule == null)
            {
                return NotFound(new { Message = "No vaccination schedules found." });
            }
            return Ok(vaccinationSchedule);
        }

        [HttpPut("vaccinations/schedules/{scheduleId}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> UpdateVaccinationSchedule(Guid scheduleId, [FromBody] VaccinationScheduleRequest request)
        {
            // Assuming there's an update method in the service
            var isUpdated = await service.UpdateScheduleAsync(scheduleId, request);
            if (!isUpdated)
            {
                return BadRequest(new { Message = "Failed to update vaccination schedule." });
            }
            return Ok("Update schedule successfully!");
        }
    }
}
