using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalInventoryService
    {
        Task<PaginationResponse<MedicalInventoryDto>?> PaginationMedicalInventoriesAsync(PaginationRequest? pagination);
    }
}
