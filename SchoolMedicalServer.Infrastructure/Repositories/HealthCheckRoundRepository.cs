using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Data;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class HealthCheckRoundRepository(SchoolMedicalManagementContext _context) : IHealthCheckRoundRepository
    {
        public async Task<int> CountByNurseIdAsync(Guid nurseId)
        {
            return await _context.HealthCheckRounds
                .CountAsync(round => round.NurseId == nurseId);
        }

        public async Task CreateHealthCheckRoundAsync(HealthCheckRound request)
        {
            await _context.HealthCheckRounds.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<HealthCheckRound?> GetHealthCheckRoundByIdAsync(Guid id)
        {
            return await _context.HealthCheckRounds
                .Include(round => round.Schedule)
                .Include(round => round.HealthCheckResults)
                .FirstOrDefaultAsync(round => round.RoundId == id);
        }

        public async Task<IEnumerable<HealthCheckRound>> GetHealthCheckRoundsAsync()
        {
            return await _context.HealthCheckRounds
                .Include(round => round.Schedule)
                .Include(round => round.HealthCheckResults)
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthCheckRound>> GetHealthCheckRoundsByNurseIdAsync(Guid nurseId, string search, int skip, int pageSize)
        {
            return await _context.HealthCheckRounds
                .Where(round => round.NurseId == nurseId &&
                                (string.IsNullOrEmpty(search) ||
                                 round.RoundName!.Contains(search) ||
                                 round.TargetGrade!.Contains(search)))
                .Skip(skip)
                .Take(pageSize)
                .Include(round => round.Schedule)
                .Include(round => round.HealthCheckResults)
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthCheckRound>> GetHealthCheckRoundsByScheduleIdAsync(Guid scheduleId)
        {
            return await _context.HealthCheckRounds
                .Where(round => round.ScheduleId == scheduleId)
                .Include(round => round.Schedule)
                .Include(round => round.HealthCheckResults)
                .ToListAsync();
        }

        public async Task UpdateHealthCheckRound(HealthCheckRound request)
        {
            _context.HealthCheckRounds.Update(request);
            await _context.SaveChangesAsync();
        }
    }
}
