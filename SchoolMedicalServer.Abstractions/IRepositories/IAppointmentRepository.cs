using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetByStaffNurseAndDateAsync(Guid staffNurseId, DateOnly? date);
        Task<Appointment?> GetByStaffNurseAndAppointmentIdAsync(Guid staffNurseId, Guid appointmentId);
        Task<int> CountByStaffNurseIdAsync(Guid staffNurseId);
        Task<List<Appointment>> GetByStaffNursePagedAsync(
            Guid staffNurseId,
            string? search,
            string? sortBy,
            string? sortOrder,
            int skip,
            int take);
        Task<Appointment?> GetByUserAndAppointmentIdAsync(Guid userId, Guid appointmentId);
        Task<int> CountByUserIdAsync(Guid userId);
        Task<List<Appointment>> GetByUserPagedAsync(Guid userId,
            string? search,
            string? sortBy,
            string? sortOrder,
            int skip,
            int take);
        Task<bool> StaffHasAppointmentAsync(DateOnly? date);
        Task AddAsync(Appointment appointment);
        Task<Appointment?> GetByIdAsync(Guid appointmentId);
        void Update(Appointment appointment);
        Task<Appointment?> GetByIdWithStudentAsync(Guid appointmentId);
        Task<IEnumerable<Appointment>> GetAllAppointment();


    }
}
