using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class MedicalEventRepository(SchoolMedicalManagementContext _context) : IMedicalEventRepository
    {
        public async Task<int> CountAsync()
            => await _context.MedicalEvents.CountAsync();

        public async Task<int> CountByStudentIdAsync(Guid studentId)
            => await _context.MedicalEvents.Where(e => e.StudentId == studentId).CountAsync();

        public async Task<List<MedicalEvent>> GetPagedAsync(int skip, int take)
            => await _context.MedicalEvents
                .OrderByDescending(e => e.EventDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

        public async Task<List<MedicalEvent>> GetByStudentIdPagedAsync(Guid studentId, int skip, int take)
            => await _context.MedicalEvents
                .Where(e => e.StudentId == studentId)
                .OrderByDescending(e => e.EventDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

        public async Task<MedicalEvent?> GetByIdAsync(Guid eventId)
            => await _context.MedicalEvents.FirstOrDefaultAsync(e => e.EventId == eventId);

        public async Task AddAsync(MedicalEvent medicalEvent)
            => await _context.MedicalEvents.AddAsync(medicalEvent);

        public async Task<MedicalEvent?> GetByIdWithStudentAsync(Guid eventId)
        {
            return await _context.MedicalEvents
                .Include(me => me.Student)
                .FirstOrDefaultAsync(me => me.EventId == eventId);
        }

        public async Task<IEnumerable<MedicalEvent>> GetAllMedicalEvent()
        {
            return await _context.MedicalEvents
              .Include(m => m.Student)
              .Include(m => m.User)
              .Include(m => m.MedicalRequests)
              .AsNoTracking()
              .ToListAsync();
        }
    }
}
