using SchoolMedicalServer.Abstractions.Dtos;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface INurseDashboardService
    {
        Task<int> GetNurseAppointmentsDashboardAsync(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseAppointmentsDetailsDashboardAsync(Guid nurseId, DashboardRequest request);
        Task<int> GetNurseMedicalEventsDashboard(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseMedicalEventsDetailsDashboard(Guid nurseId, DashboardRequest request);
        Task<int> GetNurseMedicalRegistrationsDashboard(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseMedicalRegistrationsDetailsDashboard(Guid nurseId, DashboardRequest request);
    }
}
