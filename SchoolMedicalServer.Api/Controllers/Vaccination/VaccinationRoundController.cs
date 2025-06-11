using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Vaccination
{
    [Route("api")]
    [ApiController]
    public class VaccinationRoundController(IVaccinationRoundService service) : ControllerBase
    {
        [HttpGet("vaccination-rounds/{roundId}/students")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> GetStudentsByVacciantionRoundId([FromQuery] PaginationRequest? pagination, Guid roundId)
        {
            var vaccinationRound = await service.GetStudentsByVacciantionRoundIdAsync(pagination, roundId);
            if (vaccinationRound == null)
            {
                return NotFound(new { Message = "Vaccination round not found." });
            }
            return Ok(vaccinationRound);
        }
    }
}
