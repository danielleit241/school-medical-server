using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Schedules;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IHealthCheckScheduleService
    {
        Task<bool> CreateScheduleAsync(HealthCheckScheduleRequest request);
        Task<NotificationScheduleResponse> CreateVaccinationResultsByRounds(Guid scheduleId);
        Task<HealthCheckScheduleDetailsResponse> GetHealthCheckSchedule(Guid id);
        Task<PaginationResponse<HealthCheckScheduleResponse>> GetPaginationHealthCheckSchedule(PaginationRequest? pagination);
        Task<bool> UpdateScheduleAsync(Guid scheduleId, HealthCheckScheduleUpdateRequest request);
    }
}
