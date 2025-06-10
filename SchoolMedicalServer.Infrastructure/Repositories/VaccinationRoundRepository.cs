using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class VaccinationRoundRepository(SchoolMedicalManagementContext _context) : IVaccinationRoundRepository
    {
        public async Task CreateVaccinationRoundAsync(VaccinationRound request)
        {
            await _context.VaccinationRounds.AddAsync(request);
        }

        public async Task<VaccinationRound?> GetVaccinationRoundByIdAsync(Guid id)
        {
            return await _context.VaccinationRounds.FindAsync(id);
        }

        public async Task<IEnumerable<VaccinationRound>> GetVaccinationRoundsAsync()
        {
            return await _context.VaccinationRounds.ToListAsync();
        }

        public async Task<IEnumerable<VaccinationRound>> GetVaccinationRoundsByScheduleIdAsync(Guid scheduleId)
        {
            return await _context.VaccinationRounds
                .Where(round => round.ScheduleId == scheduleId)
                .ToListAsync();
        }

        public void UpdateVaccinationRound(Guid id, VaccinationRound request)
        {
            var existingRound = _context.VaccinationRounds.Find(id);
            //if (existingRound != null)
            //{
            //    existingRound.Date = request.Date;
            //    existingRound.Location = request.Location;
            //    existingRound.ScheduleId = request.ScheduleId;
            //    _context.VaccinationRounds.Update(existingRound);
            //}
        }
    }
}
