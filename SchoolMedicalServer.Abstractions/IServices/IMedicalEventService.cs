using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalEventService
    {
        Task<NotificationMedicalEventResponse> CreateMedicalEventAsync(MedicalEventRequest request);
        Task<IEnumerable<MedicalEventResponse>> GetAllMedicalEvent(PaginationRequest? paginationRequest);
        Task<PaginationResponse<MedicalEventResponse>?> GetAllStudentMedicalEventsAsync(PaginationRequest? paginationRequest);
        Task<MedicalEventResponse?> GetMedicalEventDetailAsync(Guid medicalEventId);
        Task<PaginationResponse<MedicalEventResponse>?> GetMedicalEventsByStudentIdAsync(PaginationRequest? paginationRequest, Guid studentId);
        Task<bool> IsEnoughQuantityAsync(List<MedicalRequestDtoRequest> medicalRequests);
    }
}
