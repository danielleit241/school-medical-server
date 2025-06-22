using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Dashboard
{
    [Route("api")]
    [ApiController]
    [Authorize(Roles = "admin, manager, nurse")]
    public class NurseDashboardController(INurseDashboardService service) : ControllerBase
    {
        [HttpGet("nurses/{nurseId}/dashboards/vaccinations")]
        public async Task<IActionResult> GetNurseVaccinationsDashboard(Guid nurseId, [FromQuery] DashboardRequest request)
        {
            var vaccinations = await service.GetNurseVaccinationsDashboardAsync(nurseId, request);
            return Ok(vaccinations);
        }

        [HttpGet("nurses/{nurseId}/dashboards/health-checks")]
        public async Task<IActionResult> GetNurseHealthChecksDashboard(Guid nurseId, [FromQuery] DashboardRequest request)
        {
            var healthChecks = await service.GetNurseHealthChecksDashboardAsync(nurseId, request);
            return Ok(healthChecks);
        }

        [HttpGet("nurses/{nurseId}/dashboards/appoiments")]
        public async Task<IActionResult> GetNurseTotalAppointments(Guid nurseId, [FromBody] DashboardRequest request)
        {
            var appoinments = await service.GetNurseAppointmentsDashboardAsync(nurseId, request);
            if (appoinments < 0)
            {
                return BadRequest("Invalid request parameters.");
            }
            return Ok(appoinments);
        }
        [HttpGet("nurses/{nurseId}/dashboards/medical-registations")]
        public async Task<IActionResult> GetNurseTotalMedicalRegistrations(Guid nurseId, [FromBody] DashboardRequest request)
        {
            var registrations = await service.GetNurseMedicalRegistrationsDashboard(nurseId, request);
            if (registrations < 0)
            {
                return BadRequest("Invalid request parameters.");
            }
            return Ok(registrations);
        }

        [HttpGet("nurses/{nurseId}/dashboards/medical-events")]
        public async Task<IActionResult> GetNurseTotalMedicalEvents(Guid nurseId, [FromBody] DashboardRequest request)
        {
            var events = await service.GetNurseMedicalEventsDashboard(nurseId, request);
            if (events < 0)
            {
                return BadRequest("Invalid request parameters.");
            }
            return Ok(events);
        }

        [HttpGet("nurses/{nurseId}/dashboards/appoiments/details")]
        public async Task<IActionResult> GetNurseTotalAppointmentsDetails(Guid nurseId, [FromBody] DashboardRequest request)
        {
            var details = await service.GetNurseAppointmentsDetailsDashboardAsync(nurseId, request);
            if (details == null || !details.Any())
            {
                return NotFound("No appointment details found for the specified nurse.");
            }
            return Ok(details);
        }

        [HttpGet("nurses/{nurseId}/dashboards/medical-registations/details")]
        public async Task<IActionResult> GetNurseTotalMedicalRegistrationsDetails(Guid nurseId, [FromBody] DashboardRequest request)
        {
            var details = await service.GetNurseMedicalRegistrationsDetailsDashboard(nurseId, request);
            if (details == null || !details.Any())
            {
                return NotFound("No medical registration details found for the specified nurse.");
            }
            return Ok(details);
        }

        [HttpGet("nurses/{nurseId}/dashboards/medical-events/details")]
        public async Task<IActionResult> GetNurseTotalMedicalEventsDetails(Guid nurseId, [FromBody] DashboardRequest request)
        {
            var details = await service.GetNurseMedicalEventsDetailsDashboard(nurseId, request);
            if (details == null || !details.Any())
            {
                return NotFound("No medical event details found for the specified nurse.");
            }
            return Ok(details);
        }
    }
}
