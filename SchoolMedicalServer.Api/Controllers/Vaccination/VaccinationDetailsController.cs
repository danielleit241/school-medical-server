using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Vaccines;
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

        [HttpGet("vaccination-details/all")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetAllVaccineDetails()
        {
            var vaccineDetails = await service.GetAllVaccineDetailsAsync();
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
        [Authorize(Roles = "admin, manager, parent")]
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

        [HttpDelete("vaccination-details/{id:guid}")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> DeleteVaccinationDetail(Guid id)
        {
            var deletedDetail = await service.DeleteVaccineDetailAsync(id);
            if (deletedDetail == null)
            {
                return NotFound($"No vaccination detail found with id {id}");
            }
            return Ok(deletedDetail);
        }
    }
}
