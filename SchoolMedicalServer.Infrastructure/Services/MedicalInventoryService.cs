using SchoolMedicalServer.Abstractions.Dtos.MedicalInventory;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Repositories;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalInventoryService(
        IBaseRepository baseRepository,
        IMedicalInventoryRepository medicalInventoryRepository
    ) : IMedicalInventoryService
    {
        public async Task<MedicalInventoryResponse?> CreateMedicalInventoryAsync(MedicalInventoryRequest request)
        {
            if (string.IsNullOrEmpty(request.ItemName) || string.IsNullOrEmpty(request.Category) || string.IsNullOrEmpty(request.UnitOfMeasure))
            {
                return null;
            }

            var entity = new MedicalInventory
            {
                ItemId = Guid.NewGuid(),
                ItemName = request.ItemName,
                Category = request.Category,
                Description = request.Description,
                QuantityInStock = request.QuantityInStock,
                UnitOfMeasure = request.UnitOfMeasure,
                MinimumStockLevel = request.MinimumStockLevel,
                MaximumStockLevel = request.MaximumStockLevel,
                LastImportDate = request.LastImportDate ?? DateTime.UtcNow,
                LastExportDate = request.LastExportDate,
                ExpiryDate = request.ExpiryDate,
                Status = request.Status
            };

            await medicalInventoryRepository.AddAsync(entity);
            await baseRepository.SaveChangesAsync();

            return ToMedicalInventoryResponse(entity);
        }

        public async Task<MedicalInventoryResponse?> DeleteMedicalInventoryAsync(Guid itemId)
        {
            var item = await medicalInventoryRepository.GetByIdAsync(itemId);
            if (item == null) return null;

            medicalInventoryRepository.Delete(item);
            await baseRepository.SaveChangesAsync();

            return ToMedicalInventoryResponse(item);
        }

        public async Task<MedicalInventoryResponse?> GetMedicalInventoryByIdAsync(Guid itemId)
        {
            var item = await medicalInventoryRepository.GetByIdAsync(itemId);
            if (item == null) return null;

            return ToMedicalInventoryResponse(item);
        }

        public async Task<PaginationResponse<MedicalInventoryResponse>?> PaginationMedicalInventoriesAsync(PaginationRequest? pagination)
        {
            var totalCount = await medicalInventoryRepository.CountAsync();
            if (totalCount == 0) return null!;

            int skip = (pagination!.PageIndex - 1) * pagination.PageSize;
            var items = await medicalInventoryRepository.GetPagedAsync(skip, pagination.PageSize);

            var result = items.Select(ToMedicalInventoryResponse).ToList();

            return new PaginationResponse<MedicalInventoryResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                totalCount,
                result
            );
        }

        public async Task<MedicalInventoryResponse?> UpdateMedicalInventoryAsync(Guid itemId, MedicalInventoryRequest request)
        {
            var item = await medicalInventoryRepository.GetByIdAsync(itemId);
            if (item == null) return null;

            item.ItemName = request.ItemName;
            item.Category = request.Category;
            item.Description = request.Description;
            item.QuantityInStock = request.QuantityInStock;
            item.UnitOfMeasure = request.UnitOfMeasure;
            item.MinimumStockLevel = request.MinimumStockLevel;
            item.MaximumStockLevel = request.MaximumStockLevel;
            item.LastImportDate = request.LastImportDate;
            item.LastExportDate = request.LastExportDate;
            item.ExpiryDate = request.ExpiryDate;
            item.Status = request.Status;

            medicalInventoryRepository.Update(item);
            await baseRepository.SaveChangesAsync();

            return ToMedicalInventoryResponse(item);
        }


        private static MedicalInventoryResponse ToMedicalInventoryResponse(MedicalInventory item)
        {
            return new MedicalInventoryResponse
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
            };
        }
    }
}