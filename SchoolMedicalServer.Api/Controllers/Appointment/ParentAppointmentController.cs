using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Appointment;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Appointment
{
    [Route("api")]
    [ApiController]
    public class ParentAppointmentController(IAppointmentService service) : ControllerBase
    {
        [HttpPost("parents/appointments")]
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

        [HttpGet("parents/{userId}/appointments")]
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

        [HttpGet("parents/{userId}/appointments/{appointmentId}")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetUserAppointment(Guid userId, Guid appointmentId)
        {
            var appointment = await service.GetUserAppointment(userId, appointmentId);

            return Ok(appointment);
        }
    }
}
