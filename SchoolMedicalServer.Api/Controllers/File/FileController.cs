using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.File
{
    [Route("api")]
    [ApiController]
    public class FileController(IFileService service) : ControllerBase
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

        [HttpGet("students/export-excel")]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> ExportStudents()
        {
            try
            {
                var fileBytes = await service.ExportStudentsExcelFileAsync();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"students_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("medical-inventories/export-excel")]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> ExportMedicalInventories()
        {
            try
            {
                var fileBytes = await service.ExportMedicalInventoriesExcelFileAsync();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"medical_inventories_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
