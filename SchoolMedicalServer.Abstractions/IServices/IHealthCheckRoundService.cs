using SchoolMedicalServer.Abstractions.Dtos.MainFlow.HealthCheck.Rounds;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IHealthCheckRoundService
    {
        Task<bool> CreateHealthCheckRoundByScheduleIdAsync(HealthCheckRoundRequest request);
        Task<HealthCheckRoundResponse> GetHealthCheckRoundByIdAsync(Guid roundId);
        Task<PaginationResponse<HealthCheckRoundResponse>> GetHealthCheckRoundsByNurseIdAsync(Guid nurseId, PaginationRequest? pagination);
        Task<IEnumerable<HealthCheckRoundParentResponse>> GetHealthCheckRoundsByUserIdAsync(Guid userId, DateOnly? start, DateOnly? end);
        Task<PaginationResponse<HealthCheckRoundStudentResponse>> GetStudentsByHealthCheckRoundIdForManagerAsync(PaginationRequest? pagination, Guid roundId);
        Task<PaginationResponse<HealthCheckRoundStudentResponse>> GetStudentsByHealthCheckRoundIdForNurseAsync(PaginationRequest? pagination, Guid roundId, Guid nurseId);
        Task<IEnumerable<HealthCheckRoundStudentResponse>> GetStudentsByHealthCheckRoundIdForNurseAsync(Guid roundId, Guid nurseId);
        Task<bool> UpdateHealthCheckRoundAsync(Guid roundId, HealthCheckRoundUpdateRequest request);
        Task<bool> UpdateHealthCheckRoundStatusAsync(Guid roundId, bool request);
    }
}
