using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.File
{
    [Route("api")]
    [ApiController]
    public class ImportFileController(IImportFileService service) : ControllerBase
    {
        [HttpPost("medical-inventories/upload-excel")]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> UploadMedicalInventories(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
                await service.UploadMedicalInventoriesExcelFile(file);
                return Ok("Upload successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

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
                await service.UploadStudentsExcelFile(file);
                return Ok("Upload successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost("vaccines/upload-excel")]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> UploadVaccines(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
                await service.UploadVaccinationDetailFile(file);
                return Ok("Upload successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
