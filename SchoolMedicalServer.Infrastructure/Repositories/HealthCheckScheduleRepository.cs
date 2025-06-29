using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Data;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class HealthCheckScheduleRepository(SchoolMedicalManagementContext _context) : IHealthCheckScheduleRepository
    {
        public async Task<int> CountAsync()
        {
            return await _context.HealthCheckSchedules.CountAsync();
        }

        public async Task CreateHealthCheckSchedule(HealthCheckSchedule request)
        {
            await _context.HealthCheckSchedules.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<HealthCheckSchedule?> GetHealthCheckScheduleByIdAsync(Guid? id)
        {
            return await _context.HealthCheckSchedules
                .Where(s => s.ScheduleId == id)
                .Include(s => s.Rounds)
                .ThenInclude(r => r.HealthCheckResults)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<HealthCheckSchedule>> GetHealthCheckSchedulesAsync()
        {
            return await _context.HealthCheckSchedules.Include(s => s.Rounds).ToListAsync();
        }

        public async Task<IEnumerable<HealthCheckSchedule>> GetHealthCheckSchedulesByDateRange(DateTime monday, DateTime sunday)
        {
            return await _context.HealthCheckSchedules
                .Include(s => s.Rounds).ThenInclude(r => r.HealthCheckResults)
                .Where(s => s.ParentNotificationEndDate >= DateOnly.FromDateTime(monday) && s.ParentNotificationEndDate <= DateOnly.FromDateTime(sunday))
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthCheckSchedule>> GetPagedHealthCheckSchedule(int skip, int take)
        {
            return await _context.HealthCheckSchedules
                .Include(s => s.Rounds)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task UpdateHealthCheckSchedule(HealthCheckSchedule request)
        {
            _context.HealthCheckSchedules.Update(request);
            await _context.SaveChangesAsync();
        }
    }
}
