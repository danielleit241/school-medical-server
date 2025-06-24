using SchoolMedicalServer.Abstractions.Dtos.MainFlows;
using SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Results;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationResultService
    {
        Task<bool?> ConfirmOrDeclineVaccination(Guid resultId, ParentConfirmationRequest request);
        Task<NotificationRequest> CreateVaccinationObservation(VaccinationObservationRequest request);
        Task<bool> CreateVaccinationResult(VaccinationResultRequest request);
        Task<bool?> GetHealthQualifiedVaccinationResult(Guid resultId);
        Task<IEnumerable<VaccinationObservationInformationResponse>> GetVaccinationObservationsByRoundId(Guid roundId);
        Task<VaccinationResultResponse> GetVaccinationResult(Guid resultId);
        Task<PaginationResponse<VaccinationResultParentResponse>> GetVaccinationResultStudentAsync(PaginationRequest? pagination, Guid studentId);
        Task<bool?> IsVaccinationConfirmed(Guid resultId);
        Task<bool> UpdateHealthQualifiedVaccinationResult(Guid resultId, bool status);
    }
}
