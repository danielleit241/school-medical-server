using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Data;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class MedicalRegistrationRepository(SchoolMedicalManagementContext _context) : IMedicalRegistrationRepository
    {
        public async Task<MedicalRegistration?> GetByIdAsync(Guid registrationId)
            => await _context.MedicalRegistrations.FirstOrDefaultAsync(m => m.RegistrationId == registrationId);

        public async Task<List<MedicalRegistration>> GetNursePagedAsync(Guid staffId, int skip, int take)
        {
            return await _context.MedicalRegistrations
                .Where(m => m.StaffNurseId == staffId)
                .OrderBy(m => m.Status == null ? 1 : (m.Status == true ? 2 : 3))
                .ThenByDescending(m => m.DateSubmitted)
                .ThenBy(m => m.DateApproved == null ? 1 : 2)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }


        public async Task<int> CountAsync()
            => await _context.MedicalRegistrations.CountAsync();

        public async Task<List<MedicalRegistration>> GetByUserPagedAsync(Guid userId, int skip, int take)
            => await _context.MedicalRegistrations
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.Status == null ? 1 : (m.Status == true ? 2 : 3))
                .ThenByDescending(m => m.DateSubmitted)
                .ThenBy(m => m.DateApproved == null ? 1 : 2)
                .Skip(skip).Take(take)
                .ToListAsync();

        public async Task<int> CountByUserAsync(Guid userId)
            => await _context.MedicalRegistrations.Where(m => m.UserId == userId).CountAsync();

        public async Task AddAsync(MedicalRegistration registration)
            => await _context.MedicalRegistrations.AddAsync(registration);

        public void Update(MedicalRegistration registration)
        {
            _context.MedicalRegistrations.Update(registration);
        }

        public async Task<MedicalRegistration?> GetApprovedByIdWithStudentAsync(Guid registrationId)
        {
            return await _context.MedicalRegistrations
                .Include(mr => mr.Student)
                .FirstOrDefaultAsync(mr => mr.RegistrationId == registrationId && (mr.Status == true || mr.Status == false));
        }

        public async Task<IEnumerable<MedicalRegistration>> GetAllMedicalRegistration()
        {
            return await _context.MedicalRegistrations
                .Include(r => r.Student)
                .Include(r => r.User)
                .Include(r => r.Details)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<MedicalRegistration>> GetByUserIdAsync(Guid userId)
        {
            return await _context.MedicalRegistrations
                .Where(r => r.UserId == userId)
                .Include(r => r.Student)
                .Include(r => r.User)
                .Include(r => r.Details)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
