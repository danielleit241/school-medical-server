using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.MedicalRegistration
{
    [Route("api")]
    [ApiController]
    public class NurseMedicalRegistrationController(IMedicalRegistrationService service) : ControllerBase
    {
        [HttpGet("nurse/medical-registrations")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetMedicalRegistrations([FromQuery] PaginationRequest? paginationRequest)
        {
            var registrations = await service.GetMedicalRegistrationsAsync(paginationRequest);
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

        [HttpPut("nurse/medical-registrations/{medicalRegistrationId}")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> ApproveMedicalRegistration(Guid medicalRegistrationId, MedicalRegistrationNurseApprovedRequest request)
        {
            var isApproved = await service.ApproveMedicalRegistration(medicalRegistrationId, request);
            if (!isApproved)
            {
                return BadRequest("Failed to approve medical registration.");
            }
            return StatusCode(204, "Update successfully");
        }
    }

}
