using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IMedicalRegistrationRepository
    {
        Task<MedicalRegistration?> GetByIdAsync(Guid registrationId);
        Task<List<MedicalRegistration>> GetPagedAsync(int skip, int take);
        Task<int> CountAsync();
        Task<List<MedicalRegistration>> GetByUserPagedAsync(Guid userId, int skip, int take);
        Task<int> CountByUserAsync(Guid userId);
        Task AddAsync(MedicalRegistration registration);
        void Update(MedicalRegistration registration);
        Task<MedicalRegistration?> GetApprovedByIdWithStudentAsync(Guid registrationId);
    }
}
