using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Vaccines;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Vaccination
{
    [Route("api")]
    [ApiController]
    public class VaccinationScheduleController(IVaccinationScheduleService service) : ControllerBase
    {
        [HttpPost("vaccinations/schedules/is-valid")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> CheckVaccinationSchedule([FromBody] VaccinationScheduleCheckRequest request)
        {
            var isValid = await service.CheckVaccinationSchedule(request);
            if (!isValid)
            {
                return NotFound(new { Message = "Students in this vaccination campaign have already been vaccinated." });
            }
            return Ok("Vaccination schedule valid.");
        }

        [HttpPut("vaccinations/schedules/finished")]
        public async Task<IActionResult> UpdateStatusScheduleAsync([FromBody] ScheduleUpdateStatusRequest request)
        {
            var isUpdated = await service.UpdateStatusSchedulesAsync(request);
            if (isUpdated == false)
            {
                return BadRequest(new { Message = "Update schedule status failed" });
            }
            return Ok(new { Status = isUpdated });
        }

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
        public async Task<IActionResult> UpdateVaccinationSchedule(Guid scheduleId, [FromBody] VaccinationScheduleUpdateRequest request)
        {
            var isUpdated = await service.UpdateScheduleAsync(scheduleId, request);
            if (!isUpdated)
            {
                return BadRequest(new { Message = "Failed to update vaccination schedule." });
            }
            return Ok("Update schedule successfully!");
        }
    }
}
