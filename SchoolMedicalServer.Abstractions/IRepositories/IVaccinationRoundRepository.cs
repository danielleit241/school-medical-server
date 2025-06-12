using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IVaccinationRoundRepository
    {
        Task CreateVaccinationRoundAsync(VaccinationRound request);
        Task<VaccinationRound?> GetVaccinationRoundByIdAsync(Guid id);
        Task<IEnumerable<VaccinationRound>> GetVaccinationRoundsAsync();
        void UpdateVaccinationRound(VaccinationRound request);
        Task<IEnumerable<VaccinationRound>> GetVaccinationRoundsByScheduleIdAsync(Guid scheduleId);
        Task<int> CountByNurseIdAsync(Guid nurseId);
        Task<IEnumerable<VaccinationRound>> GetVaccinationRoundsByNurseIdAsync(Guid nurseId, string search, int skip, int pageSize);
    }
}
