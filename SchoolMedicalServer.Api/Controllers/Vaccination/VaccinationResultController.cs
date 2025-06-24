using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.MainFlows;
using SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Results;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Vaccination
{
    [Route("api")]
    [ApiController]
    public class VaccinationResultController(IVaccinationResultService service) : ControllerBase
    {
        [HttpGet("vaccination-results/{resultId}/health-qualified")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> GetHealthQualifiedVaccinationResults(Guid resultId)
        {
            var result = await service.GetHealthQualifiedVaccinationResult(resultId);
            return Ok(result);
        }

        [HttpPut("vaccination-results/{resultId}/health-qualified")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> UpdateHealthQualifiedVaccinationResult(Guid resultId, [FromBody] bool status)
        {
            var result = await service.UpdateHealthQualifiedVaccinationResult(resultId, status);
            if (!result)
            {
                return NotFound(new { Message = "Vaccination result not found or update failed." });
            }
            return Ok(result);
        }

        [HttpPost("vaccination-results")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> CreateVaccinationResult([FromBody] VaccinationResultRequest request)
        {
            var result = await service.CreateVaccinationResult(request);
            if (!result)
            {
                return BadRequest(new { Message = "Failed to create vaccination result." });
            }
            return Ok();
        }

        [HttpPost("vaccination-results/observations")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> CreateVaccinationObservation([FromBody] VaccinationObservationRequest request)
        {
            var notification = await service.CreateVaccinationObservation(request);
            if (notification == null)
            {
                return BadRequest(new { Message = "Failed to create vaccination observation." });
            }
            return Ok(notification);
        }

        [HttpGet("vaccination-rounds/{roundId}/vaccination-results/observations")]
        [Authorize(Roles = "nurse")]
        public async Task<IActionResult> GetVaccinationObservationsByRoundId(Guid roundId)
        {
            var observations = await service.GetVaccinationObservationsByRoundId(roundId);
            if (observations == null)
            {
                return NotFound(new { Message = "No vaccination observations found for this round." });
            }
            return Ok(observations);
        }

        [HttpGet("vaccination-results/{resultId}")]
        [Authorize(Roles = "admin, manager, nurse, parent")]
        public async Task<IActionResult> GetVaccinationResult(Guid resultId)
        {
            var result = await service.GetVaccinationResult(resultId);
            if (result == null)
            {
                return NotFound(new { Message = "Vaccination result not found." });
            }
            return Ok(result);
        }

        [HttpGet("vaccination-results/{resultId}/is-confirmed")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> IsVaccinationConfirmed(Guid resultId)
        {
            var result = await service.IsVaccinationConfirmed(resultId);
            if (result == null)
            {
                return NotFound(new { Message = "Parent is not confirm or decline" });
            }
            return Ok(result);
        }

        [HttpPut("vaccination-results/{resultId}/confirm")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> ConfirmOrDeclineVaccination(Guid resultId, [FromBody] ParentConfirmationRequest request)
        {
            var result = await service.ConfirmOrDeclineVaccination(resultId, request);
            if (result == null)
            {
                return NotFound(new { Message = "Vaccination round not found." });
            }
            if (result == false)
            {
                return Ok(new { Message = "Parent is decline" });
            }
            return Ok(new { Message = "Parent is confirm" });
        }

        [HttpGet("vaccination-results/students/{studentId}")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetVaccinationResultStudent([FromQuery] PaginationRequest? pagination, Guid studentId)
        {
            var result = await service.GetVaccinationResultStudentAsync(pagination, studentId);
            if (result == null)
            {
                return NotFound(new { Message = "Student vaccination results not found." });
            }
            return Ok(result);
        }
    }
}
