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
        [HttpGet("nurses/medical-registrations")]
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

        [HttpGet("nurses/medical-registrations/{medicalRegistrationId}")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetMedicalRegistrationById(Guid medicalRegistrationId)
        {
            if (medicalRegistrationId == Guid.Empty)
            {
                return BadRequest("Medical registration ID cannot be empty.");
            }
            var registration = await service.GetMedicalRegistrationAsync(medicalRegistrationId);
            if (registration == null)
            {
                return NotFound("No medical registration found for the specified ID.");
            }
            return Ok(registration);
        }

        [HttpPut("nurses/medical-registrations/{medicalRegistrationId}/approved")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> ApproveMedicalRegistration(Guid medicalRegistrationId, MedicalRegistrationNurseApprovedRequest request)
        {
            if (medicalRegistrationId == Guid.Empty)
            {
                return BadRequest("Medical registration ID cannot be empty.");
            }
            var isApproved = await service.ApproveMedicalRegistrationAsync(medicalRegistrationId, request);
            if (!isApproved)
            {
                return BadRequest("Failed to approve medical registration.");
            }
            return StatusCode(200, "Update successfully");
        }

        [HttpPut("nurses/medical-registrations/{medicalRegistrationId}/completed")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> CompletedMedicalRegistrationDetails(Guid medicalRegistrationId, MedicalRegistrationNurseCompletedDetailsRequest request)
        {
            if (medicalRegistrationId == Guid.Empty)
            {
                return BadRequest("Medical registration ID cannot be empty.");
            }
            var isApproved = await service.CompletedMedicalRegistrationDetailsAsync(medicalRegistrationId, request);
            if (!isApproved)
            {
                return BadRequest("Failed to completed medical registration details.");
            }
            return StatusCode(204, "Update successfully");
        }
    }

}
