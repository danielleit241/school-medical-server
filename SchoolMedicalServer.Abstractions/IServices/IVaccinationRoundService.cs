using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Rounds;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationRoundService
    {
        Task<PaginationResponse<VaccinationRoundStudentResponse>> GetStudentsByVacciantionRoundIdForManagerAsync(PaginationRequest? pagination, Guid roundId);
        Task<PaginationResponse<VaccinationRoundStudentResponse>> GetStudentsByVacciantionRoundIdForNurseAsync(PaginationRequest? pagination, Guid roundId, Guid nurseId);
        Task<VaccinationRoundResponse> GetVaccinationRoundByIdAsync(Guid roundId);
        Task<IEnumerable<VaccinationRoundResponse>> GetVaccinationRoundsByScheduleIdAsync(Guid scheduleId);
        Task<PaginationResponse<VaccinationRoundResponse>> GetVaccinationRoundsByNurseIdAsync(Guid nurseId, PaginationRequest? pagination);
        Task<bool> UpdateVaccinationRoundAsync(Guid roundId, VaccinationRoundRequest request);
    }
}
