using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalInventoryService(SchoolMedicalManagementContext context) : IMedicalInventoryService
    {
        public async Task<PaginationResponse<MedicalInventoryResponse>?> PaginationMedicalInventoriesAsync(PaginationRequest? pagination)
        {
            var totalCount = await context.MedicalInventories.CountAsync();

            if (totalCount == 0)
            {
                return null!;
            }

            var items = await context.MedicalInventories
                .OrderBy(i => i.ItemName)
                .Skip((pagination!.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .AsNoTracking()
                .ToListAsync();

            var result = new List<MedicalInventoryResponse>();

            foreach (var item in items)
            {
                var dto = new MedicalInventoryResponse
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

                result.Add(dto);
            }

            return new PaginationResponse<MedicalInventoryResponse>(
                pagination.PageIndex,
                pagination.PageSize,
                totalCount,
                result
            );
        }
    }
}
