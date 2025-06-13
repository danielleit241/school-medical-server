using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Vaccination
{
    [Route("api")]
    [ApiController]
    public class VaccinationResultController(IVaccinationResultService service) : ControllerBase
    {
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
