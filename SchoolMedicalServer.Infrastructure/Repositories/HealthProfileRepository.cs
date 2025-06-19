using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class HealthProfileRepository(SchoolMedicalManagementContext _context) : IHealthProfileRepository
    {
        public async Task AddAsync(HealthProfile profile) => await _context.HealthProfiles.AddAsync(profile);
        public async Task<HealthProfile?> GetByStudentIdAsync(Guid studentId)
         => await _context.HealthProfiles.FirstOrDefaultAsync(f => f.StudentId == studentId);

        public async Task<HealthProfile?> GetByStudentIdWithVaccinationsAsync(Guid studentId)
            => await _context.HealthProfiles
                .Where(h => h.StudentId == studentId)
                .Include(h => h.VaccinationDeclarations)
                .FirstOrDefaultAsync();

        public Task UpdateAsync(HealthProfile profile)
        {
            _context.HealthProfiles.Update(profile);
            return Task.CompletedTask;
        }

        public async Task AddVaccinationDeclarationAsync(VaccinationDeclaration declaration)
            => await _context.VaccinationDeclarations.AddAsync(declaration);

        public async Task<HealthProfile?> GetHealthProfileById(Guid healthProfileId)
        {
            return await _context.HealthProfiles.FirstOrDefaultAsync(h => h.HealthProfileId == healthProfileId);
        }

        public async Task<IEnumerable<HealthProfile>> GetByIdsAsync(List<Guid> healthProfileIds)
        {
            return await _context.HealthProfiles
                .Include(h => h.Student)
                .Where(h => healthProfileIds.Contains(h.HealthProfileId))
                .ToListAsync();
        }

        public async Task<IEnumerable<HealthProfile>> GetByStudentIdsAsync(IEnumerable<Guid> students)
        {
            return await _context.HealthProfiles
                .Where(h => students.Contains(h.StudentId ?? Guid.Empty))
                .ToListAsync();
        }
    }
}
