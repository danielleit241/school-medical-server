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
        Task<NotificationVaccinationResponse> CreateVaccinationResultsByRounds(Guid scheduleId);
        Task<bool> UpdateScheduleAsync(Guid scheduleId, VaccinationScheduleRequest request);
    }
}
