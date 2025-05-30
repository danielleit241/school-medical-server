using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalRegistrationService(SchoolMedicalManagementContext context) : IMedicalRegistrationService
    {
        public async Task<bool> CreateMedicalRegistrationAsync(MedicalRegistrationRequest request)
        {
            var medicalRegistration = new MedicalRegistration
            {
                RegistrationId = Guid.NewGuid(),
                StudentId = request.StudentId,
                UserId = request.UserId,
                DateSubmitted = request.DateSubmitted ?? DateOnly.FromDateTime(DateTime.Now),
                MedicationName = request.MedicationName,
                Dosage = request.Dosage,
                Notes = request.Notes,
                ParentalConsent = request.ParentConsent,
                Status = false
            };

            context.MedicalRegistrations.Add(medicalRegistration);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<MedicalRegistrationResponse> GetMedicalRegistrationAsync(Guid medicalRegistrationId)
        {
            var medicalRegistration = await context.MedicalRegistrations.FirstOrDefaultAsync(m => m.RegistrationId == medicalRegistrationId);

            if (medicalRegistration == null)
            {
                return null;
            }

            var studentInfo = await context.Students.Where(s => s.StudentId == medicalRegistration.StudentId)
                                    .Select(s => new MedicalRegistrationStudentResponse
                                    {
                                        StudentId = s.StudentId,
                                        StudentFullName = s.FullName
                                    }).FirstOrDefaultAsync();
        }

        public Task<PaginationResponse<MedicalRegistrationResponse>> GetMedicalRegistrationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<MedicalRegistrationResponse>> GetUserMedicalRegistrationsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
