using SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Rounds;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationRoundService
    {
        Task<PaginationResponse<VaccinationRoundStudentResponse>> GetStudentsByVacciantionRoundIdForManagerAsync(PaginationRequest? pagination, Guid roundId);
        Task<PaginationResponse<VaccinationRoundStudentResponse>> GetStudentsByVacciantionRoundIdForNurseAsync(PaginationRequest? pagination, Guid roundId, Guid nurseId);
        Task<IEnumerable<VaccinationRoundParentResponse>> GetVaccinationRoundsByUserIdAsync(Guid userId, DateOnly? start, DateOnly? end);
        Task<VaccinationRoundResponse> GetVaccinationRoundByIdAsync(Guid roundId);
        Task<IEnumerable<VaccinationRoundResponse>> GetVaccinationRoundsByScheduleIdAsync(Guid scheduleId);
        Task<PaginationResponse<VaccinationRoundResponse>> GetVaccinationRoundsByNurseIdAsync(Guid nurseId, PaginationRequest? pagination);
        Task<bool> UpdateVaccinationRoundStatusAsync(Guid roundId, bool request);
        Task<bool> CreateVaccinationRegularRoundByScheduleIdAsync(VaccinationRoundRequest request);
        Task<bool> UpdateVaccinationRoundAsync(Guid roundId, VaccinationRoundUpdateRequest request);
        Task<IEnumerable<VaccinationRoundStudentResponse>> GetStudentsByVacciantionRoundIdForNurseAsync(Guid roundId, Guid nurseId);
        Task<int> GetTotalSupplementaryTotalStudentsAsync(Guid scheduleId);
    }
}
