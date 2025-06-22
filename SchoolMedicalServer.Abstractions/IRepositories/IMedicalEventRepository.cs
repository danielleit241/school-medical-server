using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IMedicalEventRepository
    {
        Task<int> CountAsync();
        Task<int> CountByStudentIdAsync(Guid studentId);
        Task<List<MedicalEvent>> GetPagedAsync(int skip, int take);
        Task<List<MedicalEvent>> GetByStudentIdPagedAsync(Guid studentId, int skip, int take);
        Task<MedicalEvent?> GetByIdAsync(Guid eventId);
        Task AddAsync(MedicalEvent medicalEvent);
        Task<MedicalEvent?> GetByIdWithStudentAsync(Guid eventId);
        Task<IEnumerable<MedicalEvent>> GetAllMedicalEvent();

    }
}
