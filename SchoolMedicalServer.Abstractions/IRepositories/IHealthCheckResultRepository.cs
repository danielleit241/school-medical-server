using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IHealthCheckResultRepository
    {
        Task<IEnumerable<HealthCheckResult>> GetAllAsync();
        Task<HealthCheckResult?> GetByIdAsync(Guid? id);
        Task Create(HealthCheckResult healthCheckResult);
        Task<IEnumerable<HealthCheckResult>> GetPagedByRoundIdAsync(Guid roundId, int skip, int take);
        Task<int> CountByRoundIdAsync(Guid roundId);
        Task UpdateAsync(HealthCheckResult healthCheckResult);
        Task<IEnumerable<HealthCheckResult?>> GetPagedStudents(Guid roundId, string search, int skip, int take);
        Task<bool> IsExistStudentByRoundId(Guid id);
        Task<List<Guid>> GetHealthProfileIdsByRoundIdsAsync(List<Guid> guids);
        Task<IEnumerable<HealthCheckResult?>> GetAllStudentsInRound(Guid roundId);
        Task<IEnumerable<HealthCheckResult?>> GetByHealthProfileIdsAsync(IEnumerable<Guid> enumerable);
        Task<List<HealthCheckResult>> GetAllStudentsInSchedule(Guid scheduleId);
        Task<int> CountByStudentIdAsync(Guid studentId);
        Task<IEnumerable<HealthCheckResult?>> GetHealthCheckRoundsByStudentIdAsync(Guid studentId, string? search, int skip, int pageSize);
    }
}
