using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IVaccinationObservationRepository
    {
        Task CreateVaccinationObservation(VaccinationObservation observation);
        Task<VaccinationObservation?> GetVaccinationObservationByIdAsync(Guid observationId);
        Task<VaccinationObservation?> GetObservationsByResultIdAsync(Guid resultId);
        Task UpdateVaccinationObservation(VaccinationObservation observation);
        Task<bool> IsExistResultIdAsync(Guid vaccinationResultId);
    }
}
