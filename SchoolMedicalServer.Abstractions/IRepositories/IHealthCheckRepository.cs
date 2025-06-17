using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IHealthCheckRepository
    {
        Task CreateHealthCheckSchedule(HealthCheckSchedule request);
        Task<IEnumerable<HealthCheckSchedule>> GetHealthCheckSchedulesAsync();
        Task<VaccinationSchedule?> GetHealthCheckScheduleByIdAsync(Guid id);
        Task UpdateHealthCheckSchedule(HealthCheckSchedule request);
        Task<int> CountAsync();
        Task<IEnumerable<HealthCheckSchedule>> GetPagedHealthCheckSchedule(int skip, int take);
    }
}
