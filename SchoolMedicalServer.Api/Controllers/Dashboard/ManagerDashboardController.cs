using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Dashboard
{
    [Route("api")]
    [ApiController]
    [Authorize(Roles = "admin, manager")]
    public class ManagerDashboardController(IManagerDashboardService service) : ControllerBase
    {
        [HttpGet("managers/dashboards/students")]
        public async Task<IActionResult> GetTotalStudents([FromQuery] DashboardRequest request)
        {
            var students = await service.GetTotalStudentsAsync(request);
            if (students == null || !students.Any())
            {
                return NotFound("No students found for the specified date range.");
            }
            return Ok(students);
        }

        [HttpGet("managers/dashboards/health-declarations")]
        public async Task<IActionResult> GetTotalHealthDeclarations([FromQuery] DashboardRequest request)
        {
            var declarations = await service.GetTotalHealthDeclarationsAsync(request);
            if (declarations == null || !declarations.Any())
            {
                return NotFound("No health declarations found for the specified date range.");
            }
            return Ok(declarations);
        }

        [HttpGet("managers/dashboards/medical-requests")]
        public async Task<IActionResult> GetTotalMedicalRequests([FromQuery] DashboardRequest request)
        {
            var requests = await service.GetTotalMedicalRequestsAsync(request);
            if (requests == null || !requests.Any())
            {
                return NotFound("No medical requests found for the specified date range.");
            }
            return Ok(requests);
        }

        [HttpGet("managers/dashboards/health-checks")]
        public async Task<IActionResult> GetTotalHealthChecks([FromQuery] DashboardRequest request)
        {
            var healthChecks = await service.GetTotalHealthCheckResultsAsync(request);
            if (healthChecks == null || !healthChecks.Any())
            {
                return NotFound("No health checks found for the specified date range.");
            }
            return Ok(healthChecks);
        }

        [HttpGet("managers/dashboards/vaccinations")]
        public async Task<IActionResult> GetTotalVaccinations([FromQuery] DashboardRequest request)
        {
            var vaccinations = await service.GetTotalVaccinationResultsAsync(request);
            if (vaccinations == null || !vaccinations.Any())
            {
                return NotFound("No vaccinations found for the specified date range.");
            }
            return Ok(vaccinations);
        }

        [HttpGet("managers/dashboards/low-stock-medicals")]
        public async Task<IActionResult> GetLowStockMedicalItems()
        {
            var lowStockItems = await service.GetLowStockMedicalItemsAsync();
            if (lowStockItems == null || !lowStockItems.Any())
            {
                return NotFound("No low stock medical items found for the specified date range.");
            }
            return Ok(lowStockItems);
        }

        [HttpGet("managers/dashboards/expiring-medicals")]
        public async Task<IActionResult> GetExpiringMedicalItems()
        {
            var expiringItems = await service.GetExpiringMedicalItemsAsync();
            if (expiringItems == null || !expiringItems.Any())
            {
                return NotFound("No expiring medical items found for the specified date range.");
            }
            return Ok(expiringItems);
        }
    }
}
