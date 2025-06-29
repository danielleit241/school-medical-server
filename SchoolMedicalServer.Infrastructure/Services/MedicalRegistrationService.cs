using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalRegistrationService(
        IBaseRepository baseRepository,
        IMedicalRegistrationRepository medicalRegistrationRepository,
        IMedicalRegistrationDetailsRepository medicalRegistrationDetailsRepository,
        IStudentRepository studentRepository,
        IUserRepository userRepository) : IMedicalRegistrationService
    {
        public async Task<NotificationRequest> ApproveMedicalRegistrationAsync(Guid medicalRegistrationId, MedicalRegistrationNurseApprovedRequest request)
        {
            var medicalRegistration = await medicalRegistrationRepository.GetByIdAsync(medicalRegistrationId);
            if (medicalRegistration == null)
            {
                return null!;
            }
            if (medicalRegistration.Status == true)
            {
                return null!;
            }
            var nurse = await userRepository.GetByIdAsync(request.StaffNurseId);
            if (nurse == null || medicalRegistration.StaffNurseId != request.StaffNurseId)
            {
                return null!;
            }
            medicalRegistration.DateApproved = request.DateApproved ?? DateOnly.FromDateTime(DateTime.UtcNow);
            medicalRegistration.Status = true;

            medicalRegistrationRepository.Update(medicalRegistration);
            await baseRepository.SaveChangesAsync();

            return new NotificationRequest
            {
                NotificationTypeId = medicalRegistration.RegistrationId,
                SenderId = medicalRegistration.StaffNurseId,
                ReceiverId = medicalRegistration.UserId,
            };
        }

        public async Task<NotificationRequest> CompletedMedicalRegistrationDetailsAsync(Guid medicalRegistrationId, MedicalRegistrationNurseCompletedDetailsRequest request)
        {
            var medicalRegistration = await medicalRegistrationRepository.GetByIdAsync(medicalRegistrationId);
            if (medicalRegistration == null || request.StaffNurseId == null || medicalRegistration.StaffNurseId != request.StaffNurseId)
                return null!;

            var medicalRegistrationDetails = await medicalRegistrationDetailsRepository.GetDetailsByRegistrationAndDoseAsync(medicalRegistrationId, request.DoseNumber!);
            if (medicalRegistrationDetails == null)
                return null!;

            medicalRegistrationDetails.StaffNurseId = request.StaffNurseId;
            medicalRegistrationDetails.DateCompleted = DateTime.UtcNow;
            medicalRegistrationDetails.IsCompleted = true;

            medicalRegistrationDetailsRepository.UpdateDetails(medicalRegistrationDetails);
            await baseRepository.SaveChangesAsync();

            return new NotificationRequest
            {
                NotificationTypeId = medicalRegistrationDetails.MedicalRegistrationDetailsId,
                SenderId = medicalRegistration.StaffNurseId,
                ReceiverId = medicalRegistration.UserId,
            };
        }

        public async Task<NotificationRequest> CreateMedicalRegistrationAsync(MedicalRegistrationRequest request)
        {
            var medicalRegistration = new MedicalRegistration
            {
                RegistrationId = Guid.NewGuid(),
                StudentId = request.MedicalRegistration.StudentId,
                UserId = request.MedicalRegistration.UserId,
                DateSubmitted = request.MedicalRegistration.DateSubmitted ?? DateOnly.FromDateTime(DateTime.UtcNow),
                MedicationName = request.MedicalRegistration.MedicationName,
                TotalDosages = request.MedicalRegistration.TotalDosages,
                PictureUrl = request.MedicalRegistration.PictureUrl,
                Notes = request.MedicalRegistration.Notes,
                ParentalConsent = request.MedicalRegistration.ParentConsent,
                StaffNurseId = request.MedicalRegistration.StaffNurseId,
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

            await medicalRegistrationRepository.AddAsync(medicalRegistration);
            await baseRepository.SaveChangesAsync();

            return new NotificationRequest
            {
                NotificationTypeId = medicalRegistration.RegistrationId,
                SenderId = medicalRegistration.UserId,
                ReceiverId = medicalRegistration.StaffNurseId,
            };
        }

        public async Task<MedicalRegistrationResponse?> GetMedicalRegistrationAsync(Guid medicalRegistrationId)
        {
            var medicalRegistration = await medicalRegistrationRepository.GetByIdAsync(medicalRegistrationId);

            if (medicalRegistration == null)
            {
                return null!;
            }

            var details = await GetDetailsAsync(medicalRegistrationId);

            var studentInfo = await GetStudentInforAsync(medicalRegistration.StudentId);

            var parentInfo = await GetParentInforAsync(medicalRegistration.UserId);

            var nurseApprovedResponse = await GetNuserInforAsync(medicalRegistration.StaffNurseId, medicalRegistration);

            if (details == null || studentInfo == null || parentInfo == null) return null;

            var response = GetResponse(medicalRegistration, details, studentInfo, parentInfo, nurseApprovedResponse!);
            return response;
        }

        public async Task<PaginationResponse<MedicalRegistrationResponse?>> GetMedicalRegistrationsAsync(PaginationRequest? paginationRequest, Guid nurseId)
        {
            var medicalRegistrations = await medicalRegistrationRepository.GetAllMedicalRegistration();
            var totalCount = medicalRegistrations.Where(mr => mr.StaffNurseId == nurseId).Count();
            if (totalCount == 0)
            {
                return null!;
            }

            int skip = (paginationRequest!.PageIndex - 1) * paginationRequest.PageSize;

            var registrations = await medicalRegistrationRepository.GetNursePagedAsync(nurseId, skip, paginationRequest.PageSize);

            var result = new List<MedicalRegistrationResponse?>();

            foreach (var medicalRegistration in registrations)
            {
                var details = await GetDetailsAsync(medicalRegistration.RegistrationId);

                var studentInfo = await GetStudentInforAsync(medicalRegistration.StudentId);

                var parentInfo = await GetParentInforAsync(medicalRegistration.UserId);

                MedicalRegistrationNurseApprovedResponse? nurseApprovedResponse = await GetNuserInforAsync(medicalRegistration.StaffNurseId, medicalRegistration);

                if (details == null || studentInfo == null || parentInfo == null) return null!;

                var response = GetResponse(medicalRegistration, details, studentInfo, parentInfo, nurseApprovedResponse!);

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
            var totalCount = await medicalRegistrationRepository.CountByUserAsync(userId);

            if (totalCount == 0)
            {
                return null!;
            }
            int skip = (paginationRequest!.PageIndex - 1) * paginationRequest.PageSize;
            var registrations = await medicalRegistrationRepository.GetByUserPagedAsync(userId, skip, paginationRequest.PageSize);

            var result = new List<MedicalRegistrationResponse?>();

            foreach (var medicalRegistration in registrations)
            {
                var details = await GetDetailsAsync(medicalRegistration.RegistrationId);

                var studentInfo = await GetStudentInforAsync(medicalRegistration.StudentId);

                var parentInfo = await GetParentInforAsync(medicalRegistration.UserId);

                var nurseApprovedResponse = await GetNuserInforAsync(medicalRegistration.StaffNurseId, medicalRegistration);

                if (details == null || studentInfo == null || parentInfo == null) return null!;

                var response = GetResponse(medicalRegistration, details, studentInfo, parentInfo, nurseApprovedResponse!);

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
            var studentInfo = await studentRepository.GetStudentInfoAsync(studentId);
            if (studentInfo == null)
                return null;

            return studentInfo;
        }
        private async Task<MedicalRegistrationParentResponse?> GetParentInforAsync(Guid? UserId)
        {
            var user = await userRepository.GetByIdAsync(UserId);
            if (user == null)
                return null;
            var parentInfo = new MedicalRegistrationParentResponse
            {
                UserId = user.UserId,
                UserFullName = user.FullName
            };
            if (parentInfo == null)
                return null!;

            return parentInfo;
        }
        private async Task<MedicalRegistrationNurseApprovedResponse?> GetNuserInforAsync(Guid? staffNurseId, MedicalRegistration medicalRegistration)
        {
            var user = await userRepository.GetByIdAsync(staffNurseId);
            if (user == null || staffNurseId == null)
            {
                return null!;
            }

            var nurseInfo = user.FullName;

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
            var medicalRegistrationDetails = await medicalRegistrationDetailsRepository.GetDetailsByRegistrationIdAsync(medicalRegistrationId);
            if (medicalRegistrationDetails == null)
            {
                return null!;
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
                    PictureUrl = medicalRegistration.PictureUrl,
                    Notes = medicalRegistration.Notes,
                    ParentConsent = medicalRegistration.ParentalConsent ?? false,
                    DateSubmitted = medicalRegistration.DateSubmitted,
                    Status = medicalRegistration.Status
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
