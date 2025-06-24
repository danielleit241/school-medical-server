using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class VaccinationResultRepository(SchoolMedicalManagementContext _context) : IVaccinationResultRepository
    {
        public async Task<int> CountByRoundIdAsync(Guid roundId)
        {
            return await _context.VaccinationResults
                .CountAsync(vr => vr.RoundId == roundId);
        }

        public async Task Create(VaccinationResult vaccinationResult)
        {
            await _context.VaccinationResults.AddAsync(vaccinationResult);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<VaccinationResult>> GetAllAsync()
        {
            return await _context.VaccinationResults
                .Include(vr => vr.Round).Include(vr => vr.HealthProfile).ThenInclude(vr => vr!.Student)
                .ToListAsync();
        }

        public async Task<VaccinationResult?> GetByIdAsync(Guid? id)
        {
            return await _context.VaccinationResults
                .Include(vr => vr.HealthProfile).ThenInclude(hp => hp!.Student).ThenInclude(s => s!.User)
                .Include(vr => vr.Round)
                .ThenInclude(r => r!.Schedule).ThenInclude(s => s!.Vaccine)
                .Include(vr => vr.VaccinationObservation)
                .FirstOrDefaultAsync(vr => vr.VaccinationResultId == id);
        }

        public async Task<IEnumerable<VaccinationResult>> GetPagedByRoundIdAsync(Guid roundId, int skip, int take)
        {
            return await _context.VaccinationResults
                .Where(vr => vr.RoundId == roundId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<VaccinationResult?>> GetPagedStudents(Guid roundId, string search, int skip, int take)
        {
            return await _context.VaccinationResults
                .Include(vr => vr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(vr => vr.RoundId == roundId &&
                            (string.IsNullOrEmpty(search) ||
                            vr.HealthProfile!.Student.FullName.Contains(search)))
                .Skip(skip)
                .Take(take)
                .ToListAsync() ?? [];
        }

        public async Task<List<Guid>> GetHealthProfileIdsByRoundIdsAsync(List<Guid> guids)
        {
            return await _context.VaccinationResults
                .Where(vr => vr.ParentConfirmed == true)
                .Where(vr => guids.Contains(vr.RoundId))
                .Select(vr => vr.HealthProfileId)
                .ToListAsync();
        }

        public async Task<bool> IsExistStudentByRoundId(Guid id)
        {
            return await _context.VaccinationResults.AnyAsync(vr => vr.RoundId == id);
        }

        public async Task UpdateAsync(VaccinationResult vaccinationResult)
        {
            _context.VaccinationResults.Update(vaccinationResult);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<VaccinationResult?>> GetAllStudentsInRound(Guid roundId)
        {
            return await _context.VaccinationResults
                .Include(vr => vr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(vr => vr.RoundId == roundId)
                .ToListAsync() ?? [];
        }

        public async Task<IEnumerable<VaccinationResult?>> GetByHealthProfileIdsAsync(IEnumerable<Guid> enumerable)
        {
            return await _context.VaccinationResults
                .Include(vr => vr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Include(vr => vr.Round)
                .Where(vr => enumerable.Contains(vr.HealthProfileId))
                .ToListAsync() ?? [];
        }

        public async Task<IEnumerable<VaccinationResult?>> GetVaccinationResultsByStudentAndVaccineAsync(Guid studentId, Guid? vaccineId)
        {
            return await _context.VaccinationResults
                .Include(vr => vr.Round).ThenInclude(r => r.Schedule).ThenInclude(s => s!.Vaccine)
                .Include(vr => vr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(vr => vr.HealthProfile!.StudentId == studentId &&
                             (vaccineId == null || vr.Round!.Schedule!.Vaccine!.VaccineId == vaccineId))
                .ToListAsync() ?? [];
        }

        public async Task<IEnumerable<VaccinationResult?>> GetByHealthProfileId(Guid healthProfileId)
        {
            return await _context.VaccinationResults
                .Include(vr => vr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(vr => vr.HealthProfileId == healthProfileId)
                .Include(vr => vr.Round).ThenInclude(r => r!.Schedule).ThenInclude(s => s!.Vaccine)
                .ToListAsync() ?? [];
        }

        public async Task<List<VaccinationResult>> GetAllStudentsInSchedule(Guid scheduleId)
        {
            return await _context.VaccinationResults
                .Include(r => r.Round).ThenInclude(r => r!.Schedule)
                .Where(r => r.Round!.ScheduleId == scheduleId)
                .Include(vr => vr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(vr => vr.Round!.ScheduleId == scheduleId)
                .ToListAsync() ?? [];
        }

        public async Task<IEnumerable<VaccinationResult?>> GetByRoundIdAsync(Guid roundId)
        {
            return await _context.VaccinationResults
                .Include(vr => vr.HealthProfile)
                .ThenInclude(hp => hp!.Student)
                .Where(vr => vr.RoundId == roundId)
                .ToListAsync() ?? [];
        }
    }
}
