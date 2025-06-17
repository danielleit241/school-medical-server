using SchoolMedicalServer.Abstractions.Dtos.HealthDeclaration;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IHealthProfileDeclarationService
    {
        Task<bool> CreateHealthDeclarationAsync(HealthProfileDeclarationRequest request);
        Task<HealthProfileDeclarationResponse?> GetHealthDeclarationAsync(Guid studentId);
        Task<bool> UpdateHealthDeclarationAsync(HealthProfileDeclarationRequest request);
    }
}
