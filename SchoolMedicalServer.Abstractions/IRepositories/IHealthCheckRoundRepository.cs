using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IHealthCheckRoundRepository
    {
        Task CreateHealthCheckRoundAsync(HealthCheckRound request);
        Task<HealthCheckRound?> GetHealthCheckRoundByIdAsync(Guid id);
        Task<IEnumerable<HealthCheckRound>> GetHealthCheckRoundsAsync();
        Task UpdateHealthCheckRound(HealthCheckRound request);
        Task<IEnumerable<HealthCheckRound>> GetHealthCheckRoundsByScheduleIdAsync(Guid scheduleId);
        Task<int> CountByNurseIdAsync(Guid nurseId);
        Task<IEnumerable<HealthCheckRound>> GetHealthCheckRoundsByNurseIdAsync(Guid nurseId, string search, int skip, int pageSize);
    }
}
