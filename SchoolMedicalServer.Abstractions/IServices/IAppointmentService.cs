using SchoolMedicalServer.Abstractions.Dtos.Appointment;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAppointmentService
    {
        Task<IEnumerable<StaffNurseDto>> GetStaffNurses();
        Task<IEnumerable<AppointmentResponse>?> GetAppointmentsByStaffNurseAndDate(Guid staffNurseId, DateOnly? dateRequest);
        Task<bool> RegisterAppointment(AppointmentRequest request);

        Task<AppointmentResponse> GetStaffNurseAppointment(Guid staffNurseId, Guid appointmentId);
        Task<PaginationResponse<AppointmentResponse>> GetStaffNurseAppointments(Guid staffNurseId, PaginationRequest? paginationRequest);

        Task<AppointmentResponse> GetUserAppointment(Guid userId, Guid appointmentId);
        Task<PaginationResponse<AppointmentResponse>> GetUserAppointments(Guid userId, PaginationRequest? paginationRequest);
        Task<bool> ApproveAppointment(Guid appointmentId, ApproveRequest request);
    }
}
