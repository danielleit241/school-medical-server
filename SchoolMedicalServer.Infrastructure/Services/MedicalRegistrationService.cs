using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalRegistrationService(SchoolMedicalManagementContext context) : IMedicalRegistrationService
    {
        public Task<bool> CreateMedicalRegistrationAsync(MedicalRegistrationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<MedicalRegistrationResponse> GetMedicalRegistrationAsync(Guid medicalRegistrationId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<MedicalRegistrationResponse>> GetMedicalRegistrationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<MedicalRegistrationResponse>> GetUserMedicalRegistrationsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
