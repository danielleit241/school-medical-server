using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IVaccinationObservationRepository
    {
        void CreateVaccinationObservation(VaccinationObservation observation);
        Task<VaccinationObservation?> GetVaccinationObservationByIdAsync(Guid observationId);
        Task<VaccinationObservation?> GetObservationsByResultIdAsync(Guid resultId);
        void UpdateVaccinationObservation(VaccinationObservation observation);
        Task<bool> IsExistResultIdAsync(Guid vaccinationResultId);
    }
}
