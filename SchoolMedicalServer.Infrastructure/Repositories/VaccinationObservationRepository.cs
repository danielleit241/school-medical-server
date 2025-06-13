using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class VaccinationObservationRepository(SchoolMedicalManagementContext _context) : IVaccinationObservationRepository
    {
        public void CreateVaccinationObservation(VaccinationObservation observation)
        {
            _context.VaccinationObservations.Add(observation);
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

        public void UpdateVaccinationObservation(VaccinationObservation observation)
        {
            _context.VaccinationObservations.Update(observation);
        }
    }
}
