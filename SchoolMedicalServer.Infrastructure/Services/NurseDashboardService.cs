using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class NurseDashboardService(
        IAppointmentRepository appointmentRepository,
        IMedicalEventRepository medicalEventRepository,
        IMedicalRegistrationRepository medicalRegistrationRepository) : INurseDashboardService
    {
        public Task<int> GetNurseAppointmentsDashboardAsync(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DashboardResponse>> GetNurseAppointmentsDetailsDashboardAsync(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNurseMedicalEventsDashboard(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DashboardResponse>> GetNurseMedicalEventsDetailsDashboard(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNurseMedicalRegistrationsDashboard(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DashboardResponse>> GetNurseMedicalRegistrationsDetailsDashboard(Guid nurseId, DashboardRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
