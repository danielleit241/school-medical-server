using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class HealthCheckNotificationService(
        INotificationRepository notificationRepository,
        IStudentRepository studentRepository,
        IHealthCheckScheduleRepository healthCheckScheduleRepository,
        IHealthCheckRoundRepository healthCheckRoundRepository,
        IHealthCheckResultRepository healthCheckResultRepository,
        INotificationHelperService helperService) : IHealthCheckNotificationService
    {
        public async Task<IEnumerable<NotificationResponse>> SendHealthCheckNotificationToParents(IEnumerable<NotificationRequest> requests)
        {
            List<NotificationResponse> responses = [];
            foreach (var request in requests)
            {
                var result = await healthCheckResultRepository.GetByIdAsync(request.NotificationTypeId);
                var round = await healthCheckRoundRepository.GetHealthCheckRoundByIdAsync(result!.RoundId);
                var schedule = await healthCheckScheduleRepository.GetHealthCheckScheduleByIdAsync(round!.ScheduleId);
                var student = await studentRepository.GetStudentByHealthProfileId(result.HealthProfileId);

                var sender = await helperService.GetSenderInformationAsync(request);
                var receiver = await helperService.GetReceiverInformationAsync(request);
                if (schedule == null || sender == null || receiver == null)
                {
                    continue;
                }
                var notification = new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    UserId = receiver.UserId,
                    SenderId = sender.UserId,
                    Title = schedule.Title,
                    Content = $"Your child {student!.FullName} has received the health check: {schedule!.HealthCheckType} on {round.StartTime?.ToString("d")}.",
                    SendDate = DateTime.UtcNow,
                    IsRead = false,
                    Type = NotificationTypes.HealthCheckUp,
                    SourceId = result.ResultId
                };
                await notificationRepository.AddAsync(notification);
                var notiInfo = helperService.GetNotificationInformation(notification);
                responses.Add(helperService.GetNotificationResponse(notiInfo, sender, receiver));
            }
            return responses;
        }

        public async Task<IEnumerable<NotificationResponse>> SendHealthCheckNotificationToNurses(IEnumerable<NotificationRequest> requests)
        {
            List<NotificationResponse> responses = [];
            foreach (var request in requests)
            {
                var schedule = await healthCheckScheduleRepository.GetHealthCheckScheduleByIdAsync(request.NotificationTypeId);
                var sender = await helperService.GetSenderInformationAsync(request);
                var receiver = await helperService.GetReceiverInformationAsync(request);
                if (schedule == null || sender == null || receiver == null)
                {
                    continue;
                }
                var nurseRounds = schedule.Rounds.ToList().Where(r => r.NurseId == request.ReceiverId).ToList();
                var notification = new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    UserId = receiver.UserId,
                    SenderId = sender.UserId,
                    Title = schedule.Title,
                    Content = $"You have been assigned to the health check schedule: {schedule.HealthCheckType} with {nurseRounds.Count} rounds.",
                    SendDate = DateTime.UtcNow,
                    IsRead = false,
                    Type = NotificationTypes.HealthCheckUp,
                    SourceId = schedule.ScheduleId
                };
                await notificationRepository.AddAsync(notification);
                var notiInfo = helperService.GetNotificationInformation(notification);
                responses.Add(helperService.GetNotificationResponse(notiInfo, sender, receiver));
            }
            return responses;
        }

        public async Task<NotificationResponse> SendHealthCheckResultNotificationToParent(NotificationRequest request)
        {
            var result = await healthCheckResultRepository.GetByIdAsync(request.NotificationTypeId);
            if (result == null)
            {
                return null!;
            }
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = request.ReceiverId,
                SenderId = request.SenderId,
                Title = "Health Check Result Notification",
                Content = $"The health check result for your child {result.HealthProfile!.Student!.FullName} is now available.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.HealthCheckResult,
                SourceId = result.ResultId
            };
            await notificationRepository.AddAsync(notification);
            var notiInfo = helperService.GetNotificationInformation(notification);
            var sender = await helperService.GetSenderInformationAsync(request);
            var receiver = await helperService.GetReceiverInformationAsync(request);
            return helperService.GetNotificationResponse(notiInfo, sender, receiver);
        }
    }
}