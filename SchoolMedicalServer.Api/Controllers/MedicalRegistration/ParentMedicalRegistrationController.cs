using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.MedicalRegistration
{
    [Route("api")]
    [ApiController]
    public class ParentMedicalRegistrationController(IMedicalRegistrationService service) : ControllerBase
    {
        [HttpPost("parents/medical-registrations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> RegisterMedicalRegistration([FromBody] MedicalRegistrationRequest request)
        {
            var isCreated = await service.CreateMedicalRegistrationAsync(request);
            if (!isCreated)
            {
                return BadRequest("Failed to create medical registration.");
            }
            return Created();
        }

        [HttpGet("parents/medical-registrations/{medicalRegistrationId}")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalRegistrationById(Guid medicalRegistrationId)
        {
            var registration = await service.GetMedicalRegistrationAsync(medicalRegistrationId);
            if (registration == null)
            {
                return NotFound("No medical registration found for the specified ID.");
            }
            return Ok(registration);
        }

        [HttpGet("parents/{userId}/medical-registrations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetUserMedicalRegistrations(Guid userId)
        {
            var registrations = await service.GetUserMedicalRegistrationsAsync(userId);
            if (registrations == null)
            {
                return NotFound("No medical registrations found for the specified user.");
            }
            return Ok(registrations);
        }
    }
}
