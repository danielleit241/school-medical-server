using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalRegistrationService
    {
        Task<bool> ApproveMedicalRegistrationAsync(Guid medicalRegistrationId, MedicalRegistrationNurseApprovedRequest request);
        Task<bool> CompletedMedicalRegistrationDetailsAsync(Guid medicalRegistrationId, MedicalRegistrationNurseCompletedDetailsRequest request);
        Task<bool> CreateMedicalRegistrationAsync(MedicalRegistrationRequest request);
        Task<MedicalRegistrationResponse?> GetMedicalRegistrationAsync(Guid medicalRegistrationId);
        Task<PaginationResponse<MedicalRegistrationResponse?>> GetMedicalRegistrationsAsync(PaginationRequest? paginationRequest); //paginantion
        Task<PaginationResponse<MedicalRegistrationResponse?>> GetUserMedicalRegistrationsAsync(PaginationRequest? paginationRequest, Guid userId); //paginantion
    }
}
