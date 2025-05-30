using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.HealthDeclaration;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.HealthDeclaration
{
    [Route("api")]
    [ApiController]
    public class ParentHealthDeclarationController(IHealthProfileDeclarationService service) : ControllerBase
    {

        [HttpGet("students/{studentId}/health-declarations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetHealthProfileDeclaration(Guid studentId)
        {
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
            return Ok();
        }
    }
}
