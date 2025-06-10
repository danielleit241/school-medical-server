using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IMedicalInventoryService
    {
        Task<PaginationResponse<MedicalInventoryResponse>?> PaginationMedicalInventoriesAsync(PaginationRequest? pagination);
        Task<MedicalInventoryResponse?> GetMedicalInventoryByIdAsync(Guid itemId);
        Task<MedicalInventoryResponse?> CreateMedicalInventoryAsync(MedicalInventoryResponse request);
        Task<MedicalInventoryResponse?> UpdateMedicalInventoryAsync(Guid itemId, MedicalInventoryResponse request);
        Task<MedicalInventoryResponse?> DeleteMedicalInventoryAsync(Guid itemId);


    }
}
