using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IHealthProfileRepository
    {
        Task AddAsync(HealthProfile profile);
        Task<HealthProfile?> GetByStudentIdAsync(Guid studentId);
        Task<HealthProfile?> GetByStudentIdWithVaccinationsAsync(Guid studentId);
        Task UpdateAsync(HealthProfile profile);
        Task AddVaccinationDeclarationAsync(VaccinationDeclaration declaration);
    }
}
