using SchoolMedicalServer.Abstractions.Dtos.Dashboard;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAdminDashboardService
    {
        Task<IEnumerable<DashboardResponse>> GetColumnDataUsersAsync(DashboardRequest request);
        Task<IEnumerable<DashboardUserRecentActionResponse>> GetRecentActionsAsync(DashboardRequest reques);
    }
}
