using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Data;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class VaccinationObservationRepository(SchoolMedicalManagementContext _context) : IVaccinationObservationRepository
    {
        public async Task CreateVaccinationObservation(VaccinationObservation observation)
        {
            _context.VaccinationObservations.Add(observation);
            await _context.SaveChangesAsync();
        }

        public async Task<VaccinationObservation?> GetObservationsByResultIdAsync(Guid resultId)
        {
            return await _context.VaccinationObservations.Include(o => o.VaccinationResult)
                .FirstOrDefaultAsync(o => o.VaccinationResultId == resultId);
        }

        public Task<VaccinationObservation?> GetVaccinationObservationByIdAsync(Guid observationId)
        {
            return _context.VaccinationObservations.Include(o => o.VaccinationResult)
                .FirstOrDefaultAsync(o => o.VaccinationObservationId == observationId);
        }

        public async Task<bool> IsExistResultIdAsync(Guid vaccinationResultId)
        {
            return await _context.VaccinationObservations
                .AnyAsync(o => o.VaccinationResultId == vaccinationResultId);
        }

        public async Task UpdateVaccinationObservation(VaccinationObservation observation)
        {
            _context.VaccinationObservations.Update(observation);
            await _context.SaveChangesAsync();
        }
    }
}
