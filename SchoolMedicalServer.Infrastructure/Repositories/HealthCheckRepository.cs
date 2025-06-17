using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class HealthCheckRepository(SchoolMedicalManagementContext _context) : IHealthCheckRepository
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

        public async Task<HealthCheckSchedule?> GetHealthCheckScheduleByIdAsync(Guid id)
        {
            return await _context.HealthCheckSchedules.Include(s => s.Rounds).Where(s => s.ScheduleId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<HealthCheckSchedule>> GetHealthCheckSchedulesAsync()
        {
            return await _context.HealthCheckSchedules.Include(s => s.Rounds).ToListAsync();
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
