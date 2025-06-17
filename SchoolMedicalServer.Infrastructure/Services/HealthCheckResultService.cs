using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class HealthCheckResultService(IHealthCheckResultRepository healthCheckResultRepository) : IHealthCheckResultService
    {
    }
}
