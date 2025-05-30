using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.MedicalRegistration
{
    [Route("api")]
    [ApiController]
    public class NurseMedicalRegistrationController(IMedicalRegistrationService service) : ControllerBase
    {
        [HttpGet("nurse/medical-registrations")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetMedicalRegistrations()
        {
            var registrations = await service.GetMedicalRegistrationsAsync();
            if (registrations == null)
            {
                return NotFound("No medical registrations found.");
            }
            return Ok(registrations);
        }

        [HttpGet("nurse/medical-registrations/{medicalRegistrationId}")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetMedicalRegistrationById(Guid medicalRegistrationId)
        {
            var registration = await service.GetMedicalRegistrationAsync(medicalRegistrationId);
            if (registration == null)
            {
                return NotFound("No medical registration found for the specified ID.");
            }
            return Ok(registration);
        }

        //[HttpPut("{medicalRegistrationId}/approve")]
        //[Authorize(Roles = "nurse")]
        //public IActionResult ApproveMedicalRegistration(Guid medicalRegistrationId)
        //{
        //    var isApproved = service.ApproveMedicalRegistration(medicalRegistrationId);
        //    if (!isApproved)
        //    {
        //        return BadRequest("Failed to approve medical registration.");
        //    }
        //    return NoContent();
        //}
    }

}
