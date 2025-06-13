using SchoolMedicalServer.Abstractions.Dtos.Vaccination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationResultService
    {
        Task<bool?> ConfirmOrDeclineVaccination(Guid resultId, ParentVaccinationConfirmationRequest request);
        Task<bool?> IsVaccinationConfirmed(Guid resultId);
    }
}
