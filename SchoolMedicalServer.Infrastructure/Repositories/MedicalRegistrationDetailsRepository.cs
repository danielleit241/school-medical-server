
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class MedicalRegistrationDetailsRepository(SchoolMedicalManagementContext _context) : IMedicalRegistrationDetailsRepository
    {
        public async Task<MedicalRegistrationDetails?> GetDetailsByRegistrationAndDoseAsync(Guid registrationId, string doseNumber)
                    => await _context.MedicalRegistrationDetails
                        .FirstOrDefaultAsync(mrd => mrd.RegistrationId == registrationId && mrd.DoseNumber == doseNumber);

        public async Task<List<MedicalRegistrationDetails>> GetDetailsByRegistrationIdAsync(Guid registrationId)
            => await _context.MedicalRegistrationDetails
                .Where(mrd => mrd.RegistrationId == registrationId)
                .ToListAsync();

        public void UpdateDetails(MedicalRegistrationDetails details)
        {
            _context.MedicalRegistrationDetails.Update(details);
        }
        public async Task<MedicalRegistrationDetails?> GetByIdWithRegistrationAndStudentAsync(Guid medicalRegistrationDetailsId)
        {
            return await _context.MedicalRegistrationDetails
                .Include(mrd => mrd.MedicalRegistration)
                .ThenInclude(mr => mr!.Student)
                .FirstOrDefaultAsync(mrd => mrd.MedicalRegistrationDetailsId == medicalRegistrationDetailsId);
        }
    }
}
