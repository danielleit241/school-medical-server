using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationScheduleService
    {
        Task<bool> ConfirmOrDeclineVaccination(Guid scheduleId, ParentVaccinationConfirmationRequest request);
        Task<NotificationVaccinationResponse> CreateScheduleAsync(VaccinationScheduleRequest request);
        Task<PaginationResponse<VaccinationScheduleResponse?>?> GetPaginationVaccinationSchedule(PaginationRequest? pagination);
        Task<VaccinationScheduleResponse?> GetVaccinationSchedule(Guid id);
    }
}
