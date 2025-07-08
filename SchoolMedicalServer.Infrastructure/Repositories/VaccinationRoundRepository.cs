using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Data;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class VaccinationRoundRepository(SchoolMedicalManagementContext _context) : IVaccinationRoundRepository
    {
        public async Task<int> CountByNurseIdAsync(Guid nurseId)
        {
            return await _context.VaccinationRounds
                .CountAsync(round => round.NurseId == nurseId);
        }

        public async Task CreateVaccinationRoundAsync(VaccinationRound request)
        {
            await _context.VaccinationRounds.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<VaccinationRound?> GetVaccinationRoundByIdAsync(Guid id)
        {
            return await _context.VaccinationRounds.Include(r => r.Schedule).FirstOrDefaultAsync(r => r.RoundId == id);
        }

        public async Task<IEnumerable<VaccinationRound>> GetVaccinationRoundsAsync()
        {
            return await _context.VaccinationRounds.ToListAsync();
        }

        public async Task<IEnumerable<VaccinationRound>> GetVaccinationRoundsByNurseIdAsync(Guid nurseId, string search, int skip, int pageSize)
        {
            return await _context.VaccinationRounds
                .Where(round => round.NurseId == nurseId &&
                               (string.IsNullOrEmpty(search) ||
                               round.RoundName!.Contains(search) ||
                               round.TargetGrade!.Contains(search)))
                .OrderByDescending(round => round.StartTime)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<VaccinationRound>> GetVaccinationRoundsByScheduleIdAsync(Guid scheduleId)
        {
            return await _context.VaccinationRounds
                .Where(round => round.ScheduleId == scheduleId)
                .OrderBy(round => round.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateVaccinationRound(VaccinationRound request)
        {
            _context.VaccinationRounds.Update(request);
            await _context.SaveChangesAsync();
        }
    }
}
