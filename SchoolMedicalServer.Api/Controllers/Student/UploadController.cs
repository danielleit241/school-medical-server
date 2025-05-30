using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Student
{
    [Route("api")]
    [ApiController]
    public class UploadController(IUploadService service) : ControllerBase
    {
        [HttpPost("students/upload-excel")]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> UploadStudents(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
                await service.UploadExcelFile(file);
                return Ok("Upload successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
