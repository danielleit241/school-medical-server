namespace SchoolMedicalServer.Api.Controllers.HealthDeclaration
{
    [Route("api")]
    [ApiController]
    public class HealthDeclarationController(IHealthProfileDeclarationService service) : ControllerBase
    {

        [HttpGet("students/{studentId}/health-declarations")]
        [Authorize(Roles = "parent, nurse")]
        public async Task<IActionResult> GetHealthProfileDeclaration(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                return BadRequest("Student ID cannot be empty.");
            }
            var response = await service.GetHealthDeclarationAsync(studentId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }



        [HttpPost("students/health-declarations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> RegisterHealthProfileDeclaration(HealthProfileDeclarationRequest request)
        {
            var isCreated = await service.CreateHealthDeclarationAsync(request);
            if (!isCreated)
            {
                return BadRequest();
            }
            return StatusCode(201, "Create successfully");
        }



        [HttpPut("students/health-declarations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> UpdateHealthProfileDeclaration([FromBody] HealthProfileDeclarationRequest request)
        {
            var isUpdated = await service.UpdateHealthDeclarationAsync(request);
            if (!isUpdated)
            {
                return BadRequest("Update failed.");
            }
            return Ok("Update successfully");
        }
    }
}
