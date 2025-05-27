using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.HealthDeclaration;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.HealthDeclaration
{
    [Route("api")]
    [ApiController]
    public class HealthDeclarationController(IHealthDeclarationService service) : ControllerBase
    {

        [HttpGet("students/{studentId}/health-declarations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetHealthDeclaration(Guid studentId)
        {

            var response = await service.GetHealthDeclarationAsync(studentId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost("students/{studentId}/health-declarations")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> RegisterHealthDeclaration(Guid studentId, HealthDeclarationRequest request)
        {
            var isCreated = await service.CreateHealthDeclarationAsync(studentId, request);
            if (!isCreated)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
