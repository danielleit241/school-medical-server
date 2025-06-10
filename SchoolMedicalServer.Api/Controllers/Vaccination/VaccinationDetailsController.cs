using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.VaccinationDetails;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Vaccination
{
    [Route("api")]
    [ApiController]
    public class VaccinationDetailsController(IVaccinationDetailsService service) : ControllerBase
    {
        [HttpGet("vaccination-details")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetVaccineDetails([FromQuery] PaginationRequest? pagination)
        {
            var vaccineDetails = await service.GetVaccineDetailsAsync(pagination);
            return Ok(vaccineDetails);
        }

        [HttpPost("vaccination-details")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> CreateVaccineDetail([FromBody] VaccinationDetailsRequest vaccineDetail)
        {
            var isCreated = await service.CreateVaccineDetailAsync(vaccineDetail);
            if (isCreated)
            {
                return StatusCode(201, "Vaccine detail created successfully.");
            }
            return BadRequest("Failed to create vaccine detail.");
        }

        [HttpGet("vaccination-details/{id}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetVaccineDetail(Guid id)
        {
            var vaccineDetail = await service.GetVaccineDetailAsync(id);
            if (vaccineDetail == null)
            {
                return NotFound("Vaccine detail not found.");
            }
            return Ok(vaccineDetail);
        }

        [HttpPut("vaccination-details/{id}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> UpdateVaccineDetail(Guid id, [FromBody] VaccinationDetailsRequest vaccineDetail)
        {
            var isUpdated = await service.UpdateVaccineDetailAsync(id, vaccineDetail);
            if (isUpdated)
            {
                return Ok("Vaccine detail updated successfully.");
            }
            return BadRequest("Failed to update vaccine detail.");
        }
    }
}
