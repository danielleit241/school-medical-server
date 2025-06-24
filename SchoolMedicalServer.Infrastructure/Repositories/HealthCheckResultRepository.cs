using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class HealthCheckResultRepository(SchoolMedicalManagementContext _context) : IHealthCheckResultRepository
    {
        public async Task<int> CountByRoundIdAsync(Guid roundId)
        {
            return await _context.HealthCheckResults
                .CountAsync(hcr => hcr.RoundId == roundId);
        }

        public async Task<int> CountByStudentIdAsync(Guid studentId)
        {
            return await _context.HealthCheckResults
                .Include(hcr => hcr.HealthProfile)
                .CountAsync(hcr => hcr.HealthProfile!.StudentId == studentId);
        }

        public async Task Create(HealthCheckResult healthCheckResult)
        {
            await _context.HealthCheckResults.AddAsync(healthCheckResult);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HealthCheckResult>> GetAllAsync()
        {
            return await _context.HealthCheckResults
                .Include(hcr => hcr.Round)
                .Include(hcr => hcr.HealthProfile).ThenInclude(hcr => hcr!.Student)
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthCheckResult?>> GetAllStudentsInRound(Guid roundId)
        {
            return await _context.HealthCheckResults
                .Include(hcr => hcr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(hcr => hcr.RoundId == roundId)
                .ToListAsync();
        }

        public async Task<List<HealthCheckResult>> GetAllStudentsInSchedule(Guid scheduleId)
        {
            return await _context.HealthCheckResults
                .Include(r => r.Round).ThenInclude(r => r!.Schedule)
                .Where(r => r.Round!.ScheduleId == scheduleId)
                .Include(vr => vr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(vr => vr.Round!.ScheduleId == scheduleId)
                .ToListAsync() ?? [];
        }

        public async Task<IEnumerable<HealthCheckResult?>> GetByHealthProfileIdsAsync(IEnumerable<Guid> enumerable)
        {
            return await _context.HealthCheckResults
                .Include(r => r.Round)
                .Include(hcr => hcr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(hcr => enumerable.Contains(hcr.HealthProfileId))
                .ToListAsync();
        }

        public async Task<HealthCheckResult?> GetByIdAsync(Guid? id)
        {
            return await _context.HealthCheckResults
                .Include(hcr => hcr.Round)
                .Include(hcr => hcr.HealthProfile).ThenInclude(hp => hp!.Student).ThenInclude(s => s!.User)
                .FirstOrDefaultAsync(hcr => hcr.ResultId == id);
        }

        public async Task<IEnumerable<HealthCheckResult?>> GetByRoundIdAsync(Guid roundId)
        {
            return await _context.HealthCheckResults
                .Include(hcr => hcr.Round)
                .Include(hcr => hcr.HealthProfile).ThenInclude(hp => hp!.Student)
                .Where(hcr => hcr.RoundId == roundId)
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthCheckResult?>> GetHealthCheckRoundsByStudentIdAsync(Guid studentId, string? search, int skip, int pageSize)
        {
            return await _context.HealthCheckResults
                .Include(hcr => hcr.Round)
                .Include(hcr => hcr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(hcr => hcr.HealthProfile!.StudentId == studentId &&
                              (string.IsNullOrEmpty(search) ||
                               hcr.HealthProfile.Student.FullName.Contains(search)))
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Guid>> GetHealthProfileIdsByRoundIdsAsync(List<Guid> guids)
        {
            return await _context.HealthCheckResults
                .Where(hcr => guids.Contains(hcr.RoundId))
                .Select(hcr => hcr.HealthProfileId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthCheckResult>> GetPagedByRoundIdAsync(Guid roundId, int skip, int take)
        {
            return await _context.HealthCheckResults
                .Where(hcr => hcr.RoundId == roundId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthCheckResult?>> GetPagedStudents(Guid roundId, string search, int skip, int take)
        {
            return await _context.HealthCheckResults
                .Include(hcr => hcr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(hcr => hcr.RoundId == roundId &&
                              (string.IsNullOrEmpty(search) ||
                               hcr.HealthProfile!.Student.FullName.Contains(search)))
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<bool> IsExistStudentByRoundId(Guid id)
        {
            return await _context.HealthCheckResults
                .AnyAsync(hcr => hcr.RoundId == id);
        }

        public async Task UpdateAsync(HealthCheckResult healthCheckResult)
        {
            _context.HealthCheckResults.Update(healthCheckResult);
            await _context.SaveChangesAsync();
        }
    }
}
