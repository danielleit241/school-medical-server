using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using System.Linq.Dynamic.Core;
namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class AppointmentRepository(SchoolMedicalManagementContext _context) : IAppointmentRepository
    {
        public async Task<List<Appointment>> GetByStaffNurseAndDateAsync(Guid staffNurseId, DateOnly? date)
            => await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Student)
                .Where(a => a.AppointmentDate == date && a.StaffNurseId == staffNurseId)
                .ToListAsync();

        public async Task<Appointment?> GetByStaffNurseAndAppointmentIdAsync(Guid staffNurseId, Guid appointmentId)
            => await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.StaffNurseId == staffNurseId && a.AppointmentId == appointmentId);
        public async Task<int> CountByStaffNurseIdAsync(Guid staffNurseId)
            => await _context.Appointments.CountAsync(a => a.StaffNurseId == staffNurseId);

        public async Task<List<Appointment>> GetByStaffNursePagedAsync(Guid staffNurseId, int skip, int take)
            => await _context.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Student)
                    .Where(a => a.StaffNurseId == staffNurseId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ThenBy(a => a.AppointmentStartTime)
                    .Skip(skip)
                    .Take(take)
                    .AsNoTracking()
                    .ToListAsync();

        public async Task<Appointment?> GetByUserAndAppointmentIdAsync(Guid userId, Guid appointmentId)
            => await _context.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Student)
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.AppointmentId == appointmentId);

        public async Task<int> CountByUserIdAsync(Guid userId)
            => await _context.Appointments.CountAsync(a => a.UserId == userId);

        public async Task<List<Appointment>> GetByUserPagedAsync(Guid userId, int skip, int take)
            => await _context.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Student)
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ThenBy(a => a.AppointmentStartTime)
                    .Skip(skip)
                    .Take(take)
                    .AsNoTracking()
                    .ToListAsync();

        public async Task<bool> StaffHasAppointmentAsync(Guid? staffNurseId, DateOnly? date, TimeOnly? start, TimeOnly? end)
            => await _context.Appointments.AnyAsync(a =>
                    a.StaffNurseId == staffNurseId &&
                    a.AppointmentDate == date &&
                    a.AppointmentStartTime < end &&
                    a.AppointmentEndTime > start
        );

        public async Task AddAsync(Appointment appointment)
            => await _context.Appointments.AddAsync(appointment);
        public async Task<Appointment?> GetByIdAsync(Guid appointmentId)
            => await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        public void Update(Appointment appointment) => _context.Appointments.Update(appointment);
        public async Task<Appointment?> GetByIdWithStudentAsync(Guid appointmentId)
        {
            return await _context.Appointments
                .Where(a => a.AppointmentId == appointmentId)
                .Include(a => a.Student)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Appointment>> GetPagedAsync(
                    bool? confirmationStatus,        // Dùng nullable bool để cho phép không lọc khi null
                    string? sortBy,
                    string? sortOrder,
                    int skip,
                    int take)
        {
            IQueryable<Appointment> query = _context.Appointments.AsNoTracking();

            // Tìm kiếm theo ConfirmationStatus (bool)
            if (confirmationStatus.HasValue)
            {
                query = query.Where(s => s.ConfirmationStatus == confirmationStatus.Value);
            }

            // Mặc định sắp xếp theo AppointmentDate tăng dần
            string defaultSort = "AppointmentDate ascending";
            string sortString = !string.IsNullOrWhiteSpace(sortBy)
                ? $"{sortBy} {(sortOrder?.ToLower() == "desc" ? "descending" : "ascending")}"
                : defaultSort;

            query = query.OrderBy(sortString);

            return await query.Skip(skip).Take(take).ToListAsync();
        }

    }
}
