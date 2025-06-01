using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class MedicalInventoryController(IMedicalInventoryService service) : ControllerBase
    {
        [HttpGet("medical-inventories")]
        [Authorize(Roles = "admin, nurse, manager")]
        public async Task<IActionResult> GetAllMedicalInventories([FromQuery] PaginationRequest? pagination)
        {
            var inventories = await service.PaginationMedicalInventoriesAsync(pagination);
            if (inventories == null)
            {
                return NotFound("No medical inventories found.");
            }
            return Ok(inventories);
        }
    }
}
