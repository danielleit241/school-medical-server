using SchoolMedicalServer.Abstractions.Dtos.MainFlows;
using SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Schedules;
using SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Vaccines;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationScheduleService
    {
        Task<bool> CreateScheduleAsync(VaccinationScheduleRequest request);
        Task<PaginationResponse<VaccinationScheduleResponse?>?> GetPaginationVaccinationSchedule(PaginationRequest? pagination);
        Task<VaccinationScheduleDetailsResponse?> GetVaccinationSchedule(Guid id);
        Task<NotificationScheduleResponse> CreateVaccinationResultsByRounds(Guid scheduleId);
        Task<bool> UpdateScheduleAsync(Guid scheduleId, VaccinationScheduleUpdateRequest request);
        Task<bool> CheckVaccinationSchedule(VaccinationCheckRequest request);
        Task<bool> UpdateStatusSchedulesAsync(ScheduleUpdateStatusRequest request);
    }
}
