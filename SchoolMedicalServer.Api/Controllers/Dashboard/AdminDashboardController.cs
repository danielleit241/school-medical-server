namespace SchoolMedicalServer.Api.Controllers.Dashboard
{
    [Route("api")]
    [ApiController]
    [Authorize(Roles = "admin, manager")]
    public class AdminDashboardController(IAdminDashboardService service) : ControllerBase
    {
        [HttpGet("admins/dashboards/users")]
        public async Task<IActionResult> GetDataUsers([FromQuery] DashboardRequest request)
        {
            var res = await service.GetColumnDataUsersAsync(request);
            if (res == null || !res.Any())
            {
                return NotFound("No data found for the specified date range.");
            }
            return Ok(res);
        }

        [HttpGet("admins/dashboards/recent-actions")]
        public async Task<IActionResult> GetUsersRecentActions()
        {
            var res = await service.GetRecentActionsAsync();
            if (res == null || !res.Any())
            {
                return NotFound("No recent actions found.");
            }
            return Ok(res);
        }
    }
}
