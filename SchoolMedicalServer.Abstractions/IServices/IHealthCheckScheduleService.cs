using SchoolMedicalServer.Abstractions.Dtos.MainFlows;
using SchoolMedicalServer.Abstractions.Dtos.MainFlows.HealthCheck.Schedules;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IHealthCheckScheduleService
    {
        Task<bool> CreateScheduleAsync(HealthCheckScheduleRequest request);
        Task<NotificationScheduleResponse> CreateVaccinationResultsByRounds(Guid scheduleId);
        Task<IEnumerable<HealthCheckScheduleDetailsResponse>> GetHealthCheckSchedule(Guid id);
        Task<PaginationResponse<HealthCheckScheduleResponse>> GetPaginationHealthCheckSchedule(PaginationRequest? pagination);
        Task<bool> UpdateScheduleAsync(Guid scheduleId, HealthCheckScheduleUpdateRequest request);
        Task<bool> UpdateStatusSchedulesAsync(ScheduleUpdateStatusRequest request);
    }
}
