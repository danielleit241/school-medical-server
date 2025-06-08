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
    }
}
