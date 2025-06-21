using SchoolMedicalServer.Abstractions.Dtos.MainFlow.HealthCheck.Results;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IHealthCheckResultService
    {
        Task<bool?> ConfirmOrDeclineHealthCheck(Guid resultId, ParentHealthCheckConfirmationRequest request);
        Task<NotificationRequest> CreateHealthCheckResultAsync(HealthCheckResultRequest request);
        Task<HealthCheckResultResponse> GetHealthCheckResultAsync(Guid resultId);
        Task<PaginationResponse<HealthCheckResultResponse>> GetHealthCheckResultsByStudentIdAsync(PaginationRequest? request, Guid studentId);
        Task<bool?> IsHealthCheckConfirmed(Guid resultId);
    }
}
