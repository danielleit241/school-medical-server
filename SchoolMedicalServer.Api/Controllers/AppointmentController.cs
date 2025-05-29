using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Appointment;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class AppointmentController(IAppointmentService service) : ControllerBase
    {
        [HttpGet("staff-nurses")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetStaffNurses()
        {
            var staffNurses = await service.GetStaffNurses();
            if (staffNurses is null)
            {
                return NotFound("No staff nurses found.");
            }
            return Ok(staffNurses);
        }

        [HttpGet("staff-nurses/{staffNurseId}/appointments")]
        [Authorize(Roles = "parent,nurse")]
        public async Task<IActionResult> GetStaffNurseAppointments(Guid staffNurseId, [FromQuery] DateOnly? dateRequest, [FromQuery] PaginationRequest? paginationRequest)
        {
            if (User.IsInRole("parent"))
            {
                var appointments = await service.GetAppointmentsByStaffNurseAndDate(staffNurseId, dateRequest);
                if (appointments is null)
                {
                    return NotFound("No appointments found for the specified staff nurse.");
                }
                return Ok(appointments);
            }

            if (User.IsInRole("nurse"))
            {
                var claimStaffIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(claimStaffIdStr, out var claimStaffId) || claimStaffId != staffNurseId)
                    return Forbid();

                if (dateRequest.HasValue)
                {
                    var appointments = await service.GetAppointmentsByStaffNurseAndDate(staffNurseId, dateRequest);
                    if (appointments is null)
                    {
                        return NotFound("No appointments found for the specified staff nurse on the given date.");
                    }
                    return Ok(appointments);
                }
                else
                {
                    var response = await service.GetStaffNurseAppointments(staffNurseId, paginationRequest);
                    if (response is null)
                    {
                        return NotFound("No appointments found for the specified staff nurse.");
                    }
                    return Ok(response);
                }
            }

            return Forbid();
        }

        [HttpPost("students/appointments")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> RegisterAppointment([FromBody] AppointmentRequest request)
        {
            var isCreated = await service.RegisterAppointment(request);
            if (!isCreated)
            {
                return BadRequest("Failed to register appointment. Please check the request data.");
            }
            return Created();

        }

        [HttpGet("users/{userId}/appointments")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetUserAppointments(Guid userId, [FromQuery] PaginationRequest? paginationRequest)
        {
            var userAppointments = await service.GetUserAppointments(userId, paginationRequest);
            if (userAppointments is null || !userAppointments.Items.Any())
            {
                return NotFound("No appointments found for the specified user.");
            }
            return Ok(userAppointments);
        }

        [HttpGet("users/{userId}/appointments/{appointmentId}")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetUserAppointment(Guid userId, Guid appointmentId)
        {
            var appointment = await service.GetUserAppointment(userId, appointmentId);

            return Ok(appointment);
        }

        [HttpGet("staff-nurses/{staffNurseId}/appointments/{appointmentId}")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetStaffNurseAppointment(Guid staffNurseId, Guid appointmentId)
        {
            var appointment = await service.GetStaffNurseAppointment(staffNurseId, appointmentId);

            return Ok(appointment);
        }
    }
}
