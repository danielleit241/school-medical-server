using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IVaccinationRoundRepository
    {
        Task CreateVaccinationRoundAsync(VaccinationRound request);
        Task<VaccinationRound?> GetVaccinationRoundByIdAsync(Guid id);
        Task<IEnumerable<VaccinationRound>> GetVaccinationRoundsAsync();
        void UpdateVaccinationRound(Guid id, VaccinationRound request);
        Task<IEnumerable<VaccinationRound>> GetVaccinationRoundsByScheduleIdAsync(Guid scheduleId);
    }
}
