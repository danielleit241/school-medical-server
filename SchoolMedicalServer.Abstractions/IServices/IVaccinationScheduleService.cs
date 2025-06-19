using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationScheduleService
    {
        Task<bool> CreateScheduleAsync(VaccinationScheduleRequest request);
        Task<PaginationResponse<VaccinationScheduleResponse?>?> GetPaginationVaccinationSchedule(PaginationRequest? pagination);
        Task<VaccinationScheduleDetailsResponse?> GetVaccinationSchedule(Guid id);
        Task<NotificationScheduleResponse> CreateVaccinationResultsByRounds(Guid scheduleId);
        Task<bool> UpdateScheduleAsync(Guid scheduleId, VaccinationScheduleUpdateRequest request);
        Task<bool> CheckVaccinationSchedule(VaccinationScheduleRequest request);
        Task<bool> UpdateStatusSchedulesAsync(ScheduleUpdateStatusRequest request);
    }
}
