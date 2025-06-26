namespace SchoolMedicalServer.Api.Controllers.File
{
    [Route("api")]
    [ApiController]
    public class ExportFileController(IExportFileService service) : ControllerBase
    {
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

        [HttpGet("vaccines/export-excel")]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> ExportVaccinationDetails()
        {
            try
            {
                var fileBytes = await service.ExportVaccinationDetailsExcelFileAsync();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"vaccination_details_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("vaccination-results/export-excel")]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> ExportVaccinationResults()
        {
            try
            {
                var fileBytes = await service.ExportVaccinationResultsExcelFileAsync();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"vaccination_results_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("health-check-results/export-excel")]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> ExportHealthCheckResults()
        {
            try
            {
                var fileBytes = await service.ExportHealthCheckResultsExcelFileAsync();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"health_check_results_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
