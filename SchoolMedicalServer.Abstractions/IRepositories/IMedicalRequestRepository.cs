using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IMedicalRequestRepository
    {
        Task<List<MedicalRequestDtoResponse>> GetByEventIdAsync(Guid eventId);
        Task AddRangeAsync(List<MedicalRequest> requests);
    }
}
