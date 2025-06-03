using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalRegistrationService(SchoolMedicalManagementContext context) : IMedicalRegistrationService
    {
        public async Task<bool> ApproveMedicalRegistrationAsync(Guid medicalRegistrationId, MedicalRegistrationNurseApprovedRequest request)
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
            medicalRegistration.DateApproved = request.DateApproved ?? DateOnly.FromDateTime(DateTime.Now);
            medicalRegistration.Status = true;

            context.MedicalRegistrations.Update(medicalRegistration);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompletedMedicalRegistrationDetailsAsync(Guid medicalRegistrationId, MedicalRegistrationNurseCompletedDetailsRequest request)
        {
            var medicalRegistration = await context.MedicalRegistrations.FirstOrDefaultAsync(m => m.RegistrationId == medicalRegistrationId && m.StaffNurseId == request.StaffNurseId);
            if (medicalRegistration == null)
            {
                return false;
            }

            if (request.StaffNurseId == null)
            {
                return false;
            }

            var medicalRegistrationDetails = await context.MedicalRegistrationDetails.FirstOrDefaultAsync(mrd => mrd.RegistrationId == medicalRegistrationId && mrd.DoseNumber == request.DoseNumber);
            if (medicalRegistrationDetails == null)
                return false;

            medicalRegistrationDetails.StaffNurseId = request.StaffNurseId;
            medicalRegistrationDetails.DateCompleted = DateTime.UtcNow;
            medicalRegistrationDetails.IsCompleted = true;

            context.MedicalRegistrationDetails.Update(medicalRegistrationDetails);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateMedicalRegistrationAsync(MedicalRegistrationRequest request)
        {
            var medicalRegistration = new MedicalRegistration
            {
                RegistrationId = Guid.NewGuid(),
                StudentId = request.MedicalRegistration.StudentId,
                UserId = request.MedicalRegistration.UserId,
                DateSubmitted = request.MedicalRegistration.DateSubmitted ?? DateOnly.FromDateTime(DateTime.Now),
                MedicationName = request.MedicalRegistration.MedicationName,
                TotalDosages = request.MedicalRegistration.TotalDosages,
                Notes = request.MedicalRegistration.Notes,
                ParentalConsent = request.MedicalRegistration.ParentConsent,
            };

            foreach (var detail in request.MedicalRegistrationDetails)
            {
                var registrationDetail = new MedicalRegistrationDetails
                {
                    MedicalRegistrationDetailsId = Guid.NewGuid(),
                    RegistrationId = medicalRegistration.RegistrationId,
                    DoseNumber = detail.DoseNumber,
                    DoseTime = detail.DoseTime,
                    Notes = detail.Notes,
                    IsCompleted = false
                };
                medicalRegistration.Details.Add(registrationDetail);
            }

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

            var details = await GetDetailsAsync(medicalRegistrationId);

            var studentInfo = await GetStudentInforAsync(medicalRegistration.StudentId);

            var parentInfo = await GetParentInforAsync(medicalRegistration.UserId);

            var nurseApprovedResponse = await GetNuserInforAsync(medicalRegistration.StaffNurseId, medicalRegistration);

            if (details == null || studentInfo == null || parentInfo == null || nurseApprovedResponse == null) return null;

            var response = GetResponse(medicalRegistration, details, studentInfo, parentInfo, nurseApprovedResponse);
            return response;
        }

        public async Task<PaginationResponse<MedicalRegistrationResponse?>> GetMedicalRegistrationsAsync(PaginationRequest? paginationRequest)
        {
            var totalCount = await context.MedicalRegistrations.CountAsync();
            if (totalCount == 0)
            {
                return null!;
            }

            var registrations = await context.MedicalRegistrations
                .OrderByDescending(m => m.DateSubmitted)
                .Skip((paginationRequest!.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();

            var result = new List<MedicalRegistrationResponse?>();

            foreach (var medicalRegistration in registrations)
            {
                var details = await GetDetailsAsync(medicalRegistration.RegistrationId);

                var studentInfo = await GetStudentInforAsync(medicalRegistration.StudentId);

                var parentInfo = await GetParentInforAsync(medicalRegistration.UserId);

                MedicalRegistrationNurseApprovedResponse? nurseApprovedResponse = await GetNuserInforAsync(medicalRegistration.StaffNurseId, medicalRegistration);

                if (details == null || studentInfo == null || parentInfo == null || nurseApprovedResponse == null) return null;

                var response = GetResponse(medicalRegistration, details, studentInfo, parentInfo, nurseApprovedResponse);

                result.Add(response);
            }

            return new PaginationResponse<MedicalRegistrationResponse?>(
                paginationRequest.PageIndex,
                paginationRequest.PageSize,
                totalCount,
                result
            );
        }

        public async Task<PaginationResponse<MedicalRegistrationResponse?>> GetUserMedicalRegistrationsAsync(PaginationRequest? paginationRequest, Guid userId)
        {
            var totalCount = await context.MedicalRegistrations.Where(m => m.UserId == userId).CountAsync();

            if (totalCount == 0)
            {
                return null!;
            }

            var registrations = await context.MedicalRegistrations
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.DateSubmitted)
                .Skip((paginationRequest!.PageIndex - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();

            var result = new List<MedicalRegistrationResponse?>();

            foreach (var medicalRegistration in registrations)
            {
                var details = await GetDetailsAsync(medicalRegistration.RegistrationId);

                var studentInfo = await GetStudentInforAsync(medicalRegistration.StudentId);

                var parentInfo = await GetParentInforAsync(medicalRegistration.UserId);

                var nurseApprovedResponse = await GetNuserInforAsync(medicalRegistration.StaffNurseId, medicalRegistration);

                if (details == null || studentInfo == null || parentInfo == null) return null;

                var response = GetResponse(medicalRegistration, details, studentInfo, parentInfo, nurseApprovedResponse);

                result.Add(response);
            }

            return new PaginationResponse<MedicalRegistrationResponse?>(
                paginationRequest.PageIndex,
                paginationRequest.PageSize,
                totalCount,
                result
            );
        }

        private async Task<MedicalRegistrationStudentResponse?> GetStudentInforAsync(Guid? studentId)
        {
            var studentInfo = await context.Students
                    .Where(s => s.StudentId == studentId)
                    .Select(s => new MedicalRegistrationStudentResponse
                    {
                        StudentId = s.StudentId,
                        StudentFullName = s.FullName
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            if (studentInfo == null)
                return null;

            return studentInfo;
        }
        private async Task<MedicalRegistrationParentResponse?> GetParentInforAsync(Guid? UserId)
        {
            var parentInfo = await context.Users
                        .Where(u => u.UserId == UserId)
                        .Select(u => new MedicalRegistrationParentResponse
                        {
                            UserId = u.UserId,
                            UserFullName = u.FullName
                        })
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
            if (parentInfo == null)
                return null!;

            return parentInfo;
        }
        private async Task<MedicalRegistrationNurseApprovedResponse?> GetNuserInforAsync(Guid? staffNurseId, MedicalRegistration medicalRegistration)
        {
            var nurseInfo = await context.Users
                .Where(u => u.UserId == staffNurseId)
                .Select(u => u.FullName ?? string.Empty)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var nurseApprovedResponse = new MedicalRegistrationNurseApprovedResponse
            {
                StaffNurseId = medicalRegistration.StaffNurseId,
                StaffNurseFullName = nurseInfo,
                DateApproved = medicalRegistration.DateApproved
            };

            return nurseApprovedResponse;
        }
        private async Task<List<MedicalRegistrationDetailsResponse>> GetDetailsAsync(Guid medicalRegistrationId)
        {
            var medicalRegistrationDetails = await context.MedicalRegistrationDetails.Where(mrd => mrd.RegistrationId == medicalRegistrationId).ToListAsync();
            if (medicalRegistrationDetails == null)
            {
                return null;
            }

            var details = new List<MedicalRegistrationDetailsResponse>();
            foreach (var detail in medicalRegistrationDetails)
            {
                details.Add(new MedicalRegistrationDetailsResponse()
                {
                    DoseNumber = detail.DoseNumber,
                    Notes = detail.Notes,
                    IsCompleted = detail.IsCompleted,
                    DateCompleted = detail.DateCompleted,
                    DoseTime = detail.DoseTime,
                });
            }

            return details;
        }
        private MedicalRegistrationResponse GetResponse(MedicalRegistration medicalRegistration, List<MedicalRegistrationDetailsResponse> details, MedicalRegistrationStudentResponse studentInfo, MedicalRegistrationParentResponse parentInfo, MedicalRegistrationNurseApprovedResponse nurseApprovedResponse)
        {
            var response = new MedicalRegistrationResponse
            {
                MedicalRegistration = new MedicalRegistrationResponseDto
                {
                    RegistrationId = medicalRegistration.RegistrationId,
                    MedicationName = medicalRegistration.MedicationName,
                    TotalDosages = medicalRegistration.TotalDosages,
                    Notes = medicalRegistration.Notes,
                    ParentConsent = medicalRegistration.ParentalConsent ?? false,
                    DateSubmitted = medicalRegistration.DateSubmitted
                },
                MedicalRegistrationDetails = details,
                NurseApproved = nurseApprovedResponse,
                Student = studentInfo,
                Parent = parentInfo
            };

            return response;
        }
    }
}
