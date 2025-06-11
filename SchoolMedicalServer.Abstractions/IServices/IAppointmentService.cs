using SchoolMedicalServer.Abstractions.Dtos.Appointment;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IAppointmentService
    {
        Task<IEnumerable<StaffNurseInfo>> GetStaffNurses();
        Task<IEnumerable<AppointmentResponse>?> GetAppointmentsByStaffNurseAndDate(Guid staffNurseId, DateOnly? dateRequest);
        Task<NotificationRequest> RegisterAppointment(AppointmentRequest request);

        Task<AppointmentResponse> GetStaffNurseAppointment(Guid staffNurseId, Guid appointmentId);
        Task<PaginationResponse<AppointmentResponse>> GetStaffNurseAppointments(Guid staffNurseId, PaginationRequest? paginationRequest);

        Task<AppointmentResponse> GetUserAppointment(Guid userId, Guid appointmentId);
        Task<PaginationResponse<AppointmentResponse>> GetUserAppointments(Guid userId, PaginationRequest? paginationRequest);
        Task<NotificationRequest> ApproveAppointment(Guid appointmentId, AppoinmentNurseApprovedRequest request);
    }
}
