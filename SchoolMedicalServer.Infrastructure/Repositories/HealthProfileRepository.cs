using DocumentFormat.OpenXml.InkML;
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
    }
}
