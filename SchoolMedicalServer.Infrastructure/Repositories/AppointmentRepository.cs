using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Data;

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

        public async Task<List<Appointment>> GetByStaffNursePagedAsync(
                 Guid staffNurseId,
                 string? search,
                 string? sortBy,
                 string? sortOrder,
                 int skip,
                 int take)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Student)
                .Where(a => a.StaffNurseId == staffNurseId)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {

                if (bool.TryParse(search, out var confirmationStatus))
                {
                    query = query.Where(a => a.ConfirmationStatus == confirmationStatus);
                }

            }

            string defaultSort = "AppointmentDate ascending";
            string sortString = !string.IsNullOrWhiteSpace(sortBy)
                ? $"{sortBy} {(sortOrder?.ToLower() == "desc" ? "descending" : "ascending")}"
                : defaultSort;

            query = query.OrderBy(sortString);

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<Appointment?> GetByUserAndAppointmentIdAsync(Guid userId, Guid appointmentId)
            => await _context.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Student)
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.AppointmentId == appointmentId);

        public async Task<int> CountByUserIdAsync(Guid userId)
            => await _context.Appointments.CountAsync(a => a.UserId == userId);

        public async Task<List<Appointment>> GetByUserPagedAsync(Guid userId,
            string? search,
            string? sortBy,
            string? sortOrder,
            int skip,
            int take)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Where(a => a.UserId == userId)
                .Include(a => a.User)
                .Include(a => a.Student)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (bool.TryParse(search, out var boolValue))
                {
                    query = query.Where(a => a.ConfirmationStatus == boolValue);
                }
                else
                {
                    var lowerSearch = search.ToLowerInvariant();
                }
            }

            string defaultSort = "AppointmentDate ascending";
            string sortString = !string.IsNullOrWhiteSpace(sortBy)
                ? $"{sortBy} {(sortOrder?.ToLower() == "desc" ? "descending" : "ascending")}"
                : defaultSort;

            query = query.OrderBy(sortString);

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<bool> StaffHasAppointmentAsync(DateOnly? date, TimeOnly? startTime, TimeOnly? endTime)
            => await _context.Appointments
                .AnyAsync(a => a.AppointmentDate == date &&
                               a.AppointmentStartTime <= endTime &&
                               a.AppointmentEndTime >= startTime);

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

        public async Task<IEnumerable<Appointment>> GetAllAppointment()
        {
            return await _context.Appointments
                .Include(a => a.Student)
                .Include(a => a.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Appointments
                .Where(a => a.UserId == userId)
                .Include(a => a.Student)
                .Include(a => a.User)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
