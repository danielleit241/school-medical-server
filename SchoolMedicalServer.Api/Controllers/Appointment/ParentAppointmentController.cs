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
            var appointment = await service.RegisterAppointment(request);
            if (appointment == null)
            {
                return BadRequest("Failed to register appointment. Please check the request data.");
            }
            return StatusCode(201, appointment);
        }

        [HttpGet("parents/{parentId}/appointments/has-booked")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> HasBookedAppointment(Guid parentId)
        {
            if (parentId == Guid.Empty)
            {
                return BadRequest("User ID cannot be empty.");
            }
            var hasBooked = await service.HasBookedAppointment(parentId);
            if (!hasBooked)
            {
                return Ok("User has not booked any appointments.");
            }
            else
            {
                return BadRequest("User has booked appointments.");
            }
        }


        [HttpGet("parents/{userId}/appointments")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetUserAppointments(Guid userId, [FromQuery] PaginationRequest? paginationRequest)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID cannot be empty.");
            }
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
            if (userId == Guid.Empty || appointmentId == Guid.Empty)
            {
                return BadRequest("User ID and appointment ID cannot be empty.");
            }
            var appointment = await service.GetUserAppointment(userId, appointmentId);

            return Ok(appointment);
        }

        [HttpGet("parents/{userId}/appointments/total-cancel")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetTotalCancelledAppointments(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID cannot be empty.");
            }
            var totalCancelledInMonth = await service.GetUserCancelAppointmentsInMonth(userId);
            return Ok(new { TotalCancelled = totalCancelledInMonth });
        }
    }
}
