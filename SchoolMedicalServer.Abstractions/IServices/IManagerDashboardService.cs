using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.MedicalInventory;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IManagerDashboardService
    {
        Task<IEnumerable<IDictionary<string, MedicalInventoryDashboardResponse>>> GetExpiringMedicalItemsAsync();
        Task<IEnumerable<IDictionary<string, int>>> GetLowStockMedicalItemsAsync();
        Task<IEnumerable<DashboardResponse>> GetTotalHealthCheckResultsAsync(DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetTotalHealthDeclarationsAsync(DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetTotalMedicalRequestsAsync(DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetTotalStudentsAsync(DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetTotalVaccinationResultsAsync(DashboardRequest request);
    }
}
