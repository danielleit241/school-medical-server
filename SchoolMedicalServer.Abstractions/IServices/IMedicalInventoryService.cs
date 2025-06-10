using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalInventoryService
    {
        Task<PaginationResponse<MedicalInventoryResponse>?> PaginationMedicalInventoriesAsync(PaginationRequest? pagination);
        Task<MedicalInventoryResponse?> GetMedicalInventoryByIdAsync(Guid itemId);
        Task<MedicalInventoryResponse?> CreateMedicalInventoryAsync(MedicalInventoryRequest request);
        Task<MedicalInventoryResponse?> UpdateMedicalInventoryAsync(Guid itemId, MedicalInventoryRequest request);
        Task<MedicalInventoryResponse?> DeleteMedicalInventoryAsync(Guid itemId);


    }
}
