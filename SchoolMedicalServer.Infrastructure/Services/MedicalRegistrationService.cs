using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalRegistrationService(SchoolMedicalManagementContext context) : IMedicalRegistrationService
    {
        public async Task<bool> ApproveMedicalRegistration(Guid medicalRegistrationId, MedicalRegistrationNurseApprovedRequest request)
        {
            var medicalRegistration = await context.MedicalRegistrations.FirstOrDefaultAsync(m => m.RegistrationId == medicalRegistrationId);
            if (medicalRegistration == null)
            {
                return false;
            }
            if (medicalRegistration.Status == true)
            {
                return false;
            }
            if (request.StaffNurseId == null)
            {
                return false;
            }

            medicalRegistration.StaffNurseId = request.StaffNurseId;
            medicalRegistration.StaffNurseNotes = request.StaffNurseNotes;
            medicalRegistration.DateApproved = request.DateApproved ?? DateOnly.FromDateTime(DateTime.Now);
            medicalRegistration.Status = true;

            context.MedicalRegistrations.Update(medicalRegistration);
            await context.SaveChangesAsync();
            return true;
        }

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

        public async Task<MedicalRegistrationResponse?> GetMedicalRegistrationAsync(Guid medicalRegistrationId)
        {
            var medicalRegistration = await context.MedicalRegistrations.AsNoTracking().FirstOrDefaultAsync(m => m.RegistrationId == medicalRegistrationId);

            if (medicalRegistration == null)
            {
                return null!;
            }

            var studentInfo = await context.Students.Where(s => s.StudentId == medicalRegistration.StudentId)
                                    .Select(s => new MedicalRegistrationStudentResponse
                                    {
                                        StudentId = s.StudentId,
                                        StudentFullName = s.FullName
                                    })
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

            var parentInfor = await context.Users.Where(u => u.UserId == medicalRegistration.UserId)
                                    .Select(u => new MedicalRegistrationParentResponse
                                    {
                                        UserId = u.UserId,
                                        UserFullName = u.FullName
                                    })
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

            var nurseInfo = await context.Users
                                    .Where(u => u.UserId == medicalRegistration.StaffNurseId)
                                    .Select(u => u.FullName!)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

            var nurseApprovedResponse = new MedicalRegistrationNurseApprovedResponse
            {
                StaffNurseId = medicalRegistration.StaffNurseId,
                StaffNurseFullName = nurseInfo,
                StaffNurseNotes = medicalRegistration.StaffNurseNotes,
                DateApproved = medicalRegistration.DateApproved
            };

            var response = new MedicalRegistrationResponse
            {
                MedicalRegistration = new MedicalRegistrationDto
                {
                    RegistrationId = medicalRegistration.RegistrationId,
                    MedicationName = medicalRegistration.MedicationName,
                    Dosage = medicalRegistration.Dosage,
                    Notes = medicalRegistration.Notes,
                    ParentConsent = medicalRegistration.ParentalConsent ?? false,
                    DateSubmitted = medicalRegistration.DateSubmitted
                },
                NurseApproved = nurseApprovedResponse,
                Student = studentInfo,
                Parent = parentInfor
            };
            return response;
        }

        public Task<PaginationResponse<MedicalRegistrationResponse?>> GetMedicalRegistrationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResponse<MedicalRegistrationResponse?>> GetUserMedicalRegistrationsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
