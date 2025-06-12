using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class VaccinationScheduleRepository(SchoolMedicalManagementContext _context) : IVaccinationScheduleRepository
    {
        public async Task<int> CountAsync()
        {
            return await _context.VaccinationSchedules.CountAsync();
        }

        public async Task CreateVaccinationSchedule(VaccinationSchedule request)
        {
            await _context.VaccinationSchedules.AddAsync(request);
        }

        public async Task<IEnumerable<VaccinationSchedule>> GetPagedVaccinationSchedule(int skip, int take)
        {
            return await _context.VaccinationSchedules
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<VaccinationSchedule?> GetVaccinationScheduleByIdAsync(Guid id)
        {
            return await _context.VaccinationSchedules.Include(s => s.Rounds).Include(s => s.Vaccine)
                .Where(s => s.ScheduleId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<VaccinationSchedule>> GetVaccinationSchedulesAsync()
        {
            return await _context.VaccinationSchedules.ToListAsync();
        }

        public void UpdateVaccinationSchedule(Guid id, VaccinationSchedule request)
        {
            _context.VaccinationSchedules.Update(request);
        }
    }
}
