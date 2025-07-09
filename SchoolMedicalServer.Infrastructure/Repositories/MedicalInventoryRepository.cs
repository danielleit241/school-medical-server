using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Data;
using System.Linq.Dynamic.Core;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class MedicalInventoryRepository(SchoolMedicalManagementContext _context) : IMedicalInventoryRepository
    {
        public async Task<List<MedicalInventory>> GetAllAsync() => await _context.MedicalInventories.ToListAsync();
        public async Task AddAsync(MedicalInventory inventory) => await _context.MedicalInventories.AddAsync(inventory);
        public async Task<int> CountAsync() => await _context.MedicalInventories.Where(mi => mi.Status == true).CountAsync();

        public async Task<List<MedicalInventory>> GetPagedAsync(
                string? search,
                string? sortBy,
                string? sortOrder,
                int skip,
                int take)
        {
            IQueryable<MedicalInventory> query = _context.MedicalInventories.Where(mi => mi.Status == true).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(s => s.ItemName!.ToLower().Contains(lowerSearch));
            }

            string defaultSort = "ItemName ascending";
            string sortString = !string.IsNullOrWhiteSpace(sortBy)
                ? $"{sortBy} {(sortOrder?.ToLower() == "desc" ? "descending" : "ascending")}"
                : defaultSort;

            query = query.OrderBy(sortString);

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<MedicalInventory?> GetByIdAsync(Guid itemId)
            => await _context.MedicalInventories.FirstOrDefaultAsync(i => i.ItemId == itemId);

        public async Task<bool> IsEnoughQuantityAsync(Guid itemId, int requestQuantity, int? minimumStockLevel = null)
        {
            var item = await _context.MedicalInventories.FirstOrDefaultAsync(i => i.ItemId == itemId);
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
            _context.MedicalInventories.Update(inventory);
        }

        public void Delete(MedicalInventory inventory)
        {
            _context.MedicalInventories.Remove(inventory);
        }
    }
}
