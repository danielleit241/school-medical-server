using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
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
            if (request.StudentId == Guid.Empty || request.UserId == Guid.Empty)
            {
                return BadRequest("Invalid medical registration request. Please provide valid student ID, user ID.");
            }

            if (!request.ParentConsent)
            {
                return BadRequest("Parent consent is required for medical registration.");
            }

            if (request.Notes == null || request.MedicationName == null || request.Dosage == null)
            {
                return BadRequest("Invalid medical registration request. Please provide all required fields.");
            }

            var isCreated = await service.CreateMedicalRegistrationAsync(request);
            if (!isCreated)
            {
                return BadRequest("Failed to create medical registration.");
            }
            return StatusCode(201, "Create succesfully!");
        }

        [HttpGet("parents/medical-registrations/{medicalRegistrationId}")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetMedicalRegistrationById(Guid medicalRegistrationId)
        {
            if (medicalRegistrationId == Guid.Empty)
            {
                return BadRequest("Invalid medical registration ID.");
            }
            var registration = await service.GetMedicalRegistrationAsync(medicalRegistrationId);
            if (registration == null)
            {
                return NotFound("No medical registration found for the specified ID.");
            }
            return StatusCode(200, registration);
        }

        [HttpGet("parents/{userId}/medical-registrations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetUserMedicalRegistrations([FromQuery] PaginationRequest? paginationRequest, Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("Invalid user ID.");
            }
            var registrations = await service.GetUserMedicalRegistrationsAsync(paginationRequest, userId);
            if (registrations == null)
            {
                return NotFound("No medical registrations found for the specified user.");
            }
            return StatusCode(200, registrations);
        }
    }
}
