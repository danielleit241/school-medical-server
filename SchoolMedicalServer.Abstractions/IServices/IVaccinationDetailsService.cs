using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.VaccinationDetails;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IVaccinationDetailsService
    {
        Task<bool> CreateVaccineDetailAsync(VaccinationDetailsRequest vaccineDetail);
        Task<VaccinationDetailsResponse> GetVaccineDetailAsync(Guid id);
        Task<PaginationResponse<VaccinationDetailsResponse>> GetVaccineDetailsAsync(PaginationRequest? pagination);
        Task<bool> UpdateVaccineDetailAsync(Guid id, VaccinationDetailsRequest vaccineDetail);
    }
}
