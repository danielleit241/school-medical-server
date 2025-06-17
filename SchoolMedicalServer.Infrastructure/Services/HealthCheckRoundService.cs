using SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Rounds;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class HealthCheckRoundService(IHealthCheckRoundRepository healthCheckRoundRepository) : IHealthCheckRoundService
    {
        public Task<bool> CreateHealthCheckRoundByScheduleIdAsync(HealthCheckRoundRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<HealthCheckRoundStudentResponse>> GetHealthCheckRoundsByNurseIdAsync(Guid nurseId, PaginationRequest? pagination)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<HealthCheckRoundStudentResponse>> GetStudentsByHealthCheckRoundIdForManagerAsync(PaginationRequest? pagination, Guid roundId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<HealthCheckRoundStudentResponse>> GetStudentsByHealthCheckRoundIdForNurseAsync(PaginationRequest? pagination, Guid roundId, Guid nurseId)
        {
            throw new NotImplementedException();
        }
    }
}
