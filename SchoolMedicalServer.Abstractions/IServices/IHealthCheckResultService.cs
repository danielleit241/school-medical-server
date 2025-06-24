using SchoolMedicalServer.Abstractions.Dtos.MainFlows;
using SchoolMedicalServer.Abstractions.Dtos.MainFlows.HealthCheck.Results;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IHealthCheckResultService
    {
        Task<bool?> ConfirmOrDeclineHealthCheck(Guid resultId, ParentConfirmationRequest request);
        Task<NotificationRequest> CreateHealthCheckResultAsync(HealthCheckResultRequest request);
        Task<HealthCheckResultResponse> GetHealthCheckResultAsync(Guid resultId);
        Task<PaginationResponse<HealthCheckResultResponse>> GetHealthCheckResultsByStudentIdAsync(PaginationRequest? request, Guid studentId);
        Task<bool?> IsHealthCheckConfirmed(Guid resultId);
    }
}
