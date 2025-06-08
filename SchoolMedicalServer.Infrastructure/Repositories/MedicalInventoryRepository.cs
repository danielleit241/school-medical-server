using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class MedicalInventoryRepository(SchoolMedicalManagementContext context) : IMedicalInventoryRepository
    {
        public async Task<List<MedicalInventory>> GetAllAsync() => await context.MedicalInventories.ToListAsync();
        public async Task AddAsync(MedicalInventory inventory) => await context.MedicalInventories.AddAsync(inventory);
        public async Task<int> CountAsync() => await context.MedicalInventories.CountAsync();

        public async Task<List<MedicalInventory>> GetPagedAsync(int skip, int take) => await context.MedicalInventories
                .OrderBy(i => i.ItemName)
                .Skip(skip)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        public async Task<MedicalInventory?> GetByIdAsync(Guid itemId)
            => await context.MedicalInventories.FirstOrDefaultAsync(i => i.ItemId == itemId);

        public async Task<bool> IsEnoughQuantityAsync(Guid itemId, int requestQuantity, int? minimumStockLevel = null)
        {
            var item = await context.MedicalInventories.FirstOrDefaultAsync(i => i.ItemId == itemId);
            if (item == null)
                return false;
            if (item.QuantityInStock < requestQuantity)
                return false;
            if (minimumStockLevel.HasValue && (item.QuantityInStock - requestQuantity) < minimumStockLevel.Value)
                return false;
            return true;
        }

        public void Update(MedicalInventory inventory)
        {
            context.MedicalInventories.Update(inventory);
        }
    }
}
