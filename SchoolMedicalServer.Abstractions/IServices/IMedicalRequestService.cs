using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalRequestService
    {
        Task<PaginationResponse<MedicalRequestResponse>> GetMedicalRequestsAsync(PaginationRequest? pagination);
    }
}
