using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination;
using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Results;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationResultService
    {
        Task<bool?> ConfirmOrDeclineVaccination(Guid resultId, ParentVaccinationConfirmationRequest request);
        Task<NotificationRequest> CreateVaccinationObservation(VaccinationObservationRequest request);
        Task<bool> CreateVaccinationResult(VaccinationResultRequest request);
        Task<VaccinationResultResponse> GetVaccinationResult(Guid resultId);
        Task<bool?> IsVaccinationConfirmed(Guid resultId);
    }
}
