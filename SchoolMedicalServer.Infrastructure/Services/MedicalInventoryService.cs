using SchoolMedicalServer.Abstractions.Dtos.MedicalInventory;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalInventoryService(
        IMedicalInventoryRepository medicalInventoryRepository) : IMedicalInventoryService
    {
        public async Task<PaginationResponse<MedicalInventoryResponse>?> PaginationMedicalInventoriesAsync(PaginationRequest? pagination)
        {
            var totalCount = await medicalInventoryRepository.CountAsync();
            if (totalCount == 0) return null!;

            int skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var items = await medicalInventoryRepository.GetPagedAsync(skip, pagination.PageSize);

            var result = items.Select(item => new MedicalInventoryResponse
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                Category = item.Category,
                Description = item.Description,
                QuantityInStock = item.QuantityInStock,
                UnitOfMeasure = item.UnitOfMeasure,
                MinimumStockLevel = item.MinimumStockLevel,
                MaximumStockLevel = item.MaximumStockLevel,
                LastImportDate = item.LastImportDate,
                LastExportDate = item.LastExportDate,
                ExpiryDate = item.ExpiryDate,
                Status = item.Status
            }).ToList();

            return new PaginationResponse<MedicalInventoryResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                totalCount,
                result
            );
        }
    }
}
