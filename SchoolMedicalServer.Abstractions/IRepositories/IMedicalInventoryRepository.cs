using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IRepositories
{
    public interface IMedicalInventoryRepository
    {
        Task<List<MedicalInventory>> GetAllAsync();
        Task AddAsync(MedicalInventory inventory);
        Task<int> CountAsync();
        Task<List<MedicalInventory>> GetPagedAsync(int skip, int take);
        Task<MedicalInventory?> GetByIdAsync(Guid itemId);
        Task<bool> IsEnoughQuantityAsync(Guid itemId, int requestQuantity, int? minimumStockLevel = null);
        void Update(MedicalInventory inventory);
    }
}
