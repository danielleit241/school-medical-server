using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Vaccines;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationScheduleService
    {
        Task<bool> CreateScheduleAsync(VaccinationScheduleRequest request);
        Task<PaginationResponse<VaccinationScheduleResponse?>?> GetPaginationVaccinationSchedule(PaginationRequest? pagination);
        Task<VaccinationScheduleDetailsResponse?> GetVaccinationSchedule(Guid id);
        Task<NotificationScheduleResponse> CreateVaccinationResultsByRounds(Guid scheduleId);
        Task<bool> UpdateScheduleAsync(Guid scheduleId, VaccinationScheduleUpdateRequest request);
        Task<bool> CheckVaccinationSchedule(VaccinationScheduleCheckRequest request);
        Task<bool> UpdateStatusSchedulesAsync(ScheduleUpdateStatusRequest request);
    }
}
