using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalEventService
    {
        Task<bool> CreateMedicalEventAsync(MedicalEventRequest request);
        Task<PaginationResponse<MedicalEventResponse>?> GetAllStudentMedicalEventsAsync(PaginationRequest? paginationRequest);
        Task<MedicalEventResponse?> GetMedicalEventDetailAsync(PaginationRequest? paginationRequest, Guid medicalEventId);
        Task<PaginationResponse<MedicalEventResponse>?> GetMedicalEventsByStudentIdAsync(PaginationRequest? paginationRequest, Guid studentId);
        Task<bool> IsEnoughQuantityAsync(List<MedicalRequestDtoRequest> medicalRequests);
    }
}
