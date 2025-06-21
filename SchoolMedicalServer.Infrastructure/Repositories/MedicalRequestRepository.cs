using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.MedicalEvent;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class MedicalRequestRepository(SchoolMedicalManagementContext context) : IMedicalRequestRepository
    {
        public async Task<List<MedicalRequestDtoResponse>> GetByEventIdAsync(Guid eventId)
        {
            return await context.MedicalRequests
                .Where(r => r.MedicalEventId == eventId)
                .Join(context.MedicalInventories,
                      r => r.ItemId,
                      i => i.ItemId,
                      (r, i) => new MedicalRequestDtoResponse
                      {
                          RequestId = r.RequestId,
                          ItemId = r.ItemId,
                          ItemName = i.ItemName,
                          RequestQuantity = r.RequestQuantity
                      })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddRangeAsync(List<MedicalRequest> requests)
            => await context.MedicalRequests.AddRangeAsync(requests);

        public async Task<int> CountAsync()
        {
            return await context.MedicalRequests
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<IEnumerable<MedicalRequest>> GetMedicalRequestsAsync(int pageSize, int skip, string? search)
        {
            return await context.MedicalRequests
                .Include(r => r.Item)
                .Include(r => r.Event)
                .Where(r => string.IsNullOrEmpty(search) ||
                            r.Item!.ItemName!.Contains(search) ||
                            r.Purpose!.Contains(search))
                .OrderByDescending(r => r.RequestDate)
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRequest>> GetAllAsync()
        {
            return await context.MedicalRequests
                .Include(r => r.Item)
                .Include(r => r.Event)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
