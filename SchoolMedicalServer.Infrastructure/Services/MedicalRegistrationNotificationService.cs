using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalRegistrationNotificationService(
        INotificationRepository notificationRepository,
        IMedicalRegistrationRepository medicalRegistrationRepository,
        IMedicalRegistrationDetailsRepository medicalRegistrationDetailsRepository,
        IStudentRepository studentRepository,
        INotificationHelperService helperService) : IMedicalRegistrationNotificationService
    {
        public async Task<NotificationResponse> SendMedicalRegistrationNotificationToNurseAsync(NotificationRequest request)
        {
            var medicalRegistration = await medicalRegistrationRepository.GetByIdAsync(request.NotificationTypeId);
            if (medicalRegistration == null)
            {
                return null!;
            }
            var receiver = await helperService.GetReceiverInformationAsync(request);
            var sender = await helperService.GetSenderInformationAsync(request);
            var student = await studentRepository.GetStudentInfoAsync(medicalRegistration.StudentId);
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "New Medical Registration Notification",
                Content = $"A new medical registration for {student!.StudentFullName} has been created by {sender.UserName} with medication: {medicalRegistration.MedicationName}. Please review it.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.MedicalRegistration,
                SourceId = medicalRegistration.RegistrationId
            };
            await notificationRepository.AddAsync(notification);
            var notiInfo = helperService.GetNotificationInformation(notification);
            return helperService.GetNotificationResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> SendMedicalRegistrationApprovedNotificationToParentAsync(NotificationRequest request)
        {
            var medicalRegistration = await medicalRegistrationRepository.GetApprovedByIdWithStudentAsync(request.NotificationTypeId);

            if (medicalRegistration == null)
            {
                return null!;
            }

            var receiver = await helperService.GetReceiverInformationAsync(request);
            var sender = await helperService.GetSenderInformationAsync(request);

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "Medical Registration Approved",
                Content = $"Your child's medication registration ({medicalRegistration.MedicationName}) has been approved by the nurse.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.MedicalRegistration,
                SourceId = medicalRegistration.RegistrationId
            };
            await notificationRepository.AddAsync(notification);
            var notiInfo = helperService.GetNotificationInformation(notification);
            return helperService.GetNotificationResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> SendMedicalRegistrationCompletedNotificationToParentAsync(NotificationRequest request)
        {
            var medicalRegistrationDetail = await medicalRegistrationDetailsRepository.GetByIdWithRegistrationAndStudentAsync(request.NotificationTypeId);

            if (medicalRegistrationDetail == null)
            {
                return null!;
            }

            var receiver = await helperService.GetReceiverInformationAsync(request);
            var sender = await helperService.GetSenderInformationAsync(request);

            var medicationName = medicalRegistrationDetail.MedicalRegistration?.MedicationName ?? "The medication";
            var doseNumber = !string.IsNullOrEmpty(medicalRegistrationDetail.DoseNumber) ? $" (Dose {medicalRegistrationDetail.DoseNumber})" : "";
            var doseTime = !string.IsNullOrEmpty(medicalRegistrationDetail.DoseTime) ? $" at {medicalRegistrationDetail.DoseTime}" : "";
            var studentName = medicalRegistrationDetail.MedicalRegistration?.Student?.FullName ?? "your child";

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "Medication Dose Completed",
                Content = $"A dose{doseNumber} of medication ({medicationName}) for {studentName}{doseTime} has been completed by the nurse.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.MedicalRegistration,
                SourceId = medicalRegistrationDetail.MedicalRegistrationDetailsId
            };
            await notificationRepository.AddAsync(notification);
            var notiInfo = helperService.GetNotificationInformation(notification);
            return helperService.GetNotificationResponse(notiInfo, sender, receiver);
        }
    }
}