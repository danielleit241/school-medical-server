using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos;

namespace SchoolMedicalServer.Api
{
    [Route("api")]
    [ApiController]
    public class VaccinationController : ControllerBase
    {
        [HttpPost("vaccinations/schedules")]
        [Authorize(Roles = "manager")]
        public IActionResult CreateVaccinationSchedule([FromBody] VaccinationScheduleRequest request)
        {
            // Logic to create a vaccination schedule
            return Ok(new { Message = "Vaccination schedule created successfully." });
        }
    }
}
