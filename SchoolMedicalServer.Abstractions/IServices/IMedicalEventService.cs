using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalEventService
    {
        Task<MedicalEvent> CreateMedicalEventAsync(MedicalEventRequest request);
        Task<PaginationResponse<MedicalEventResponse>?> GetAllStudentMedicalEventsAsync(PaginationRequest? paginationRequest);
        Task<MedicalEventResponse?> GetMedicalEventDetailAsync(Guid medicalEventId);
        Task<PaginationResponse<MedicalEventResponse>?> GetMedicalEventsByStudentIdAsync(PaginationRequest? paginationRequest, Guid studentId);
        Task<bool> IsEnoughQuantityAsync(List<MedicalRequestDtoRequest> medicalRequests);
    }
}
