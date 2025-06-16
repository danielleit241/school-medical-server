using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IVaccinationResultRepository
    {
        Task<IEnumerable<VaccinationResult>> GetAllAsync();
        Task<VaccinationResult?> GetByIdAsync(Guid? id);
        Task Create(VaccinationResult vaccinationResult);
        Task<IEnumerable<VaccinationResult>> GetPagedByRoundIdAsync(Guid roundId, int skip, int take);
        Task<int> CountByRoundIdAsync(Guid roundId);
        Task UpdateAsync(VaccinationResult vaccinationResult);
        Task<IEnumerable<VaccinationResult?>> GetPagedStudents(Guid roundId, string search, int skip, int take);
        Task<bool> IsExistStudentByRoundId(Guid id);
        Task<List<Guid>> GetHealthProfileIdsByRoundIdsAsync(List<Guid> guids);
        Task<IEnumerable<VaccinationResult?>> GetAllStudentsInRound(Guid roundId);
    }
}
