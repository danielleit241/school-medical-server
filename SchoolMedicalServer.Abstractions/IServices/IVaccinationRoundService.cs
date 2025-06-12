using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Rounds;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationRoundService
    {
        Task<PaginationResponse<VaccinationRoundStudentResponse>> GetStudentsByVacciantionRoundIdAsync(PaginationRequest? pagination, Guid roundId);
        Task<VaccinationRoundResponse> GetVaccinationRoundByIdAsync(Guid roundId);
        Task<IEnumerable<VaccinationRoundResponse>> GetVaccinationRoundsByScheduleIdAsync(Guid scheduleId);
        Task<PaginationResponse<VaccinationRoundResponse>> GetVaccinationRoundsByNurseIdAsync(Guid nurseId, PaginationRequest? pagination);
        Task<bool> UpdateVaccinationRoundAsync(Guid roundId, VaccinationRoundRequest request);
    }
}
