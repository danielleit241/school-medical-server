using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalRegistrationService
    {
        Task<bool> ApproveMedicalRegistration(Guid medicalRegistrationId, MedicalRegistrationNurseApprovedRequest request);
        Task<bool> CreateMedicalRegistrationAsync(MedicalRegistrationRequest request);
        Task<MedicalRegistrationResponse?> GetMedicalRegistrationAsync(Guid medicalRegistrationId);
        Task<PaginationResponse<MedicalRegistrationResponse?>> GetMedicalRegistrationsAsync(); //paginantion
        Task<PaginationResponse<MedicalRegistrationResponse?>> GetUserMedicalRegistrationsAsync(Guid userId); //paginantion
    }
}
