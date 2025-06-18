
using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Results;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IHealthCheckResultService
    {
        Task<bool?> ConfirmOrDeclineHealthCheck(Guid resultId, ParentHealthCheckConfirmationRequest request);
        Task<bool> CreateHealthCheckResultAsync(HealthCheckResultRequest request);
        Task<HealthCheckResultResponse> GetHealthCheckResultAsync(Guid resultId);
        Task<bool?> IsHealthCheckConfirmed(Guid resultId);
    }
}
