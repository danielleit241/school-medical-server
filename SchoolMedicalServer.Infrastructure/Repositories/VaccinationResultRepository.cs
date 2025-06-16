using System.Threading.Tasks;
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
                .Include(vr => vr.Round)
                .ToListAsync();
        }

        public async Task<VaccinationResult?> GetByIdAsync(Guid? id)
        {
            return await _context.VaccinationResults
                .Include(vr => vr.Round)
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
    }
}
