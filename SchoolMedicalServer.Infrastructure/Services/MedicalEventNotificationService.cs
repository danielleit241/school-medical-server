using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class MedicalEventNotificationService(
        INotificationRepository notificationRepository,
        IMedicalEventRepository medicalEventRepository,
        INotificationHelperService helperService,
        IMedicalRequestRepository medicalRequestRepository) : IMedicalEventNotificationService
    {
        public async Task<NotificationResponse> SendMedicalEventNotificationToManagerAsync(NotificationRequest request)
        {
            var medicalRequests = await medicalRequestRepository.GetAllAsync();
            medicalRequests = medicalRequests.Where(r => r.MedicalEventId == request.NotificationTypeId).ToList();

            var receiver = await helperService.GetReceiverInformationAsync(request);
            var sender = await helperService.GetSenderInformationAsync(request);
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "Medical Event Notification",
                Content = $"A medical event has been recorded for a student with {medicalRequests.Count()} medical requests.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.MedicalEvent,
                SourceId = request.NotificationTypeId
            };
            await notificationRepository.AddAsync(notification);
            var notiInfo = helperService.GetNotificationInformation(notification);
            var notificationResponse = helperService.GetNotificationResponse(notiInfo, sender, receiver);

            return notificationResponse;
        }

        public async Task<NotificationResponse> SendMedicalEventNotificationToParentAsync(NotificationRequest request)
        {
            var medicalEvent = await medicalEventRepository.GetByIdWithStudentAsync(request.NotificationTypeId);
            if (medicalEvent == null)
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
                Title = "Medical Event Notification",
                Content = $"A medical event has been recorded for {medicalEvent.Student?.FullName} on {medicalEvent.EventDate?.ToString("d")}.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.MedicalEvent,
                SourceId = medicalEvent.EventId
            };
            await notificationRepository.AddAsync(notification);
            var notiInfo = helperService.GetNotificationInformation(notification);
            return helperService.GetNotificationResponse(notiInfo, sender, receiver);
        }
    }
}