using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Rounds;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IHealthCheckRoundService
    {
        Task<bool> CreateHealthCheckRoundByScheduleIdAsync(HealthCheckRoundRequest request);
        Task<PaginationResponse<HealthCheckRoundStudentResponse>> GetHealthCheckRoundsByNurseIdAsync(Guid nurseId, PaginationRequest? pagination);
        Task<PaginationResponse<HealthCheckRoundStudentResponse>> GetStudentsByHealthCheckRoundIdForManagerAsync(PaginationRequest? pagination, Guid roundId);
        Task<PaginationResponse<HealthCheckRoundStudentResponse>> GetStudentsByHealthCheckRoundIdForNurseAsync(PaginationRequest? pagination, Guid roundId, Guid nurseId);
    }
}
