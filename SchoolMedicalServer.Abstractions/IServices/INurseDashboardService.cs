using SchoolMedicalServer.Abstractions.Dtos.Dashboard;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface INurseDashboardService
    {
        Task<DashboardResponse> GetNurseAppointmentsDashboardAsync(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseAppointmentsDetailsDashboardAsync(Guid nurseId, DashboardRequest request);
        Task<DashboardResponse> GetNurseMedicalEventsDashboard(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseMedicalEventsDetailsDashboard(Guid nurseId, DashboardRequest request);
        Task<DashboardResponse> GetNurseMedicalRegistrationsDashboard(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardResponse>> GetNurseMedicalRegistrationsDetailsDashboard(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardRoundResponse>> GetNurseVaccinationsDashboardAsync(Guid nurseId, DashboardRequest request);
        Task<IEnumerable<DashboardRoundResponse>> GetNurseHealthChecksDashboardAsync(Guid nurseId, DashboardRequest request);

    }
}
