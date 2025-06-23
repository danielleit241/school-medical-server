using SchoolMedicalServer.Abstractions.Dtos;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAdminDashboardService
    {
        Task<IEnumerable<DashboardResponse>> GetColumnDataUsersAsync(DashboardRequest request);
        Task<IEnumerable<DashboardUserRecentActionResponse>> GetRecentActionsAsync();
    }
}
