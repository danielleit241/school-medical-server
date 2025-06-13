using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Results;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Vaccination
{
    [Route("api")]
    [ApiController]
    public class VaccinationResultController(IVaccinationResultService service) : ControllerBase
    {
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

        [HttpGet("vaccination-results/{resultId}")]
        [Authorize(Roles = "admin, manager, nurse")]
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

        [HttpPut("vaccination-results/{resultId}/comfirm")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> ConfirmOrDeclineVaccination(Guid resultId, [FromBody] ParentVaccinationConfirmationRequest request)
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
    }
}
