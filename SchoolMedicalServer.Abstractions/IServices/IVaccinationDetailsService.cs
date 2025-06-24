using SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Vaccines;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationDetailsService
    {
        Task<bool> CreateVaccineDetailAsync(VaccinationDetailsRequest vaccineDetail);
        Task<VaccinationDetailsResponse> GetVaccineDetailAsync(Guid id);
        Task<PaginationResponse<VaccinationDetailsResponse>> GetVaccineDetailsAsync(PaginationRequest? pagination);
        Task<bool> UpdateVaccineDetailAsync(Guid id, VaccinationDetailsRequest vaccineDetail);
        Task<VaccinationDetailsResponse> DeleteVaccineDetailAsync(Guid id);
        Task<IEnumerable<VaccinationDetailsResponse>> GetAllVaccineDetailsAsync();
    }
}
