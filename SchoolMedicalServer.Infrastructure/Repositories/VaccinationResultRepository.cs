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

        public async Task<bool> IsExistStudentByRoundId(Guid id)
        {
            return await _context.VaccinationResults.AnyAsync(vr => vr.RoundId == id);
        }

        public void Update(VaccinationResult vaccinationResult)
        {
            _context.VaccinationResults.Update(vaccinationResult);
        }
    }
}
