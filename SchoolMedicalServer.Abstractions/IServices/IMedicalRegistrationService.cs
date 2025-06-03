using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalRegistrationService
    {
        Task<MedicalRegistration> ApproveMedicalRegistrationAsync(Guid medicalRegistrationId, MedicalRegistrationNurseApprovedRequest request);
        Task<MedicalRegistrationDetails> CompletedMedicalRegistrationDetailsAsync(Guid medicalRegistrationId, MedicalRegistrationNurseCompletedDetailsRequest request);
        Task<MedicalRegistration> CreateMedicalRegistrationAsync(MedicalRegistrationRequest request);
        Task<MedicalRegistrationResponse?> GetMedicalRegistrationAsync(Guid medicalRegistrationId);
        Task<PaginationResponse<MedicalRegistrationResponse?>> GetMedicalRegistrationsAsync(PaginationRequest? paginationRequest); //paginantion
        Task<PaginationResponse<MedicalRegistrationResponse?>> GetUserMedicalRegistrationsAsync(PaginationRequest? paginationRequest, Guid userId); //paginantion
    }
}
