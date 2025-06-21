using SchoolMedicalServer.Abstractions.Dtos;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface INurseDashboardService
    {
        Task<IEnumerable<DashboardResponse>> GetNurseAppointmentsDashboardAsync(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseAppointmentsDetailsDashboardAsync(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseMedicalEventsDashboard(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseMedicalEventsDetailsDashboard(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseMedicalRegistrationsDashboard(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseMedicalRegistrationsDetailsDashboard(Guid nurseId, DashboardRequest request);
    }
}
