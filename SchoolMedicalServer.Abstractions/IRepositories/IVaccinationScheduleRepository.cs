using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IVaccinationScheduleRepository
    {
        Task CreateVaccinationSchedule(VaccinationSchedule request);
        Task<IEnumerable<VaccinationSchedule>> GetVaccinationSchedulesAsync();
        Task<VaccinationSchedule?> GetVaccinationScheduleByIdAsync(Guid id);
        void UpdateVaccinationSchedule(VaccinationSchedule request);
        Task<int> CountAsync();
        Task<IEnumerable<VaccinationSchedule>> GetPagedVaccinationSchedule(int skip, int take);
        Task<IEnumerable<VaccinationSchedule>> GetVaccinationSchedulesByDateRange(DateTime monday, DateTime sunday);
    }
}
