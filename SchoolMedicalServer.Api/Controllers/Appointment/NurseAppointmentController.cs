using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Appointment;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Appointment
{
    [Route("api")]
    [ApiController]
    public class NurseAppointmentController(IAppointmentService service) : ControllerBase
    {
        [HttpGet("nurses")]
        [Authorize]
        public async Task<IActionResult> GetStaffNurses()
        {
            var staffNurses = await service.GetStaffNurses();
            if (staffNurses is null)
            {
                return NotFound("No staff nurses found.");
            }
            return Ok(staffNurses);
        }

        [HttpGet("nurses/{staffNurseId}/appointments")]
        [Authorize(Roles = "nurse, parent")]
        public async Task<IActionResult> GetStaffNurseAppointments(Guid staffNurseId, [FromQuery] DateOnly? dateRequest, [FromQuery] PaginationRequest? paginationRequest)
        {
            if (staffNurseId == Guid.Empty)
            {
                return BadRequest("Staff nurse ID cannot be empty.");
            }
            if (dateRequest.HasValue)
            {
                var appointments = await service.GetAppointmentsByStaffNurseAndDate(staffNurseId, dateRequest);
                if (appointments is null)
                {
                    return NotFound("No appointments found for the specified staff nurse on the given date.");
                }
                return Ok(appointments);
            }

            var response = await service.GetStaffNurseAppointments(staffNurseId, paginationRequest);
            if (response is null)
            {
                return NotFound("No appointments found for the specified staff nurse.");
            }
            return Ok(response);
        }

        [HttpGet("nurses/{staffNurseId}/appointments/{appointmentId}")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetStaffNurseAppointment(Guid staffNurseId, Guid appointmentId)
        {
            if (staffNurseId == Guid.Empty || appointmentId == Guid.Empty)
            {
                return BadRequest("Staff nurse ID and appointment ID cannot be empty.");
            }
            var appointment = await service.GetStaffNurseAppointment(staffNurseId, appointmentId);

            return Ok(appointment);
        }

        [HttpPut("nurses/appointments/{appointmentId}")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> UpdateAppointment(Guid appointmentId, [FromBody] AppoinmentNurseApprovedRequest request)
        {
            if (appointmentId == Guid.Empty)
            {
                return BadRequest("Appointment ID cannot be empty.");
            }
            var appointment = await service.ApproveAppointment(appointmentId, request);
            if (appointment == null)
            {
                return BadRequest("Failed to update appointment. Please check the request data.");
            }
            return StatusCode(204, appointment);
        }
    }
}
