using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalEventService
    {
        Task<bool> CreateMedicalEventAsync(MedicalEventRequest request);
        Task<PaginationResponse<MedicalEventResponse>?> GetAllStudentMedicalEventsAsync();
        Task<MedicalEventResponse?> GetMedicalEventDetailAsync(Guid medicalEventId);
        Task<PaginationResponse<MedicalEventResponse>?> GetMedicalEventsByStudentIdAsync(Guid studentId);
        Task<bool> IsEnoughQuantityAsync(List<MedicalRequestDtoRequest> medicalRequests);
    }
}
