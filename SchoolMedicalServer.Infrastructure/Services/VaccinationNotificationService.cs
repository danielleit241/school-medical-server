using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class VaccinationNotificationService(
        INotificationRepository notificationRepository,
        IStudentRepository studentRepository,
        IVaccinationResultRepository resultRepository,
        IVaccinationRoundRepository roundRepository,
        IVaccinationScheduleRepository scheduleRepository,
        IVaccinationObservationRepository observationRepository,
        IHealthProfileRepository profileRepository,
        INotificationHelperService helperService) : IVaccinationNotificationService
    {
        public async Task<IEnumerable<NotificationResponse>> SendVaccinationResultNotificationToParents(IEnumerable<NotificationRequest> requests)
        {
            List<NotificationResponse> responses = [];
            foreach (var request in requests)
            {
                try
                {
                    var result = await resultRepository.GetByIdAsync(request.NotificationTypeId);
                    var round = await roundRepository.GetVaccinationRoundByIdAsync(result!.RoundId);
                    var schedule = await scheduleRepository.GetVaccinationScheduleByIdAsync(round!.ScheduleId);
                    var sender = await helperService.GetSenderInformationAsync(request);
                    var receiver = await helperService.GetReceiverInformationAsync(request);

                    var notification = new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        UserId = receiver.UserId,
                        SenderId = sender.UserId,
                        Title = schedule!.Title,
                        Content = $"Your child {result.HealthProfile?.Student!.FullName} has received the vaccination: {schedule.Vaccine!.VaccineName} on {round.StartTime?.ToString("d")}.",
                        SendDate = DateTime.UtcNow,
                        IsRead = false,
                        Type = NotificationTypes.Vaccination,
                        SourceId = result.VaccinationResultId
                    };
                    await notificationRepository.AddAsync(notification);
                    var notiInfo = helperService.GetNotificationInformation(notification);
                    responses.Add(helperService.GetNotificationResponse(notiInfo, sender, receiver));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            if (responses.Count == 0)
            {
                return null!;
            }
            return responses;
        }

        public async Task<IEnumerable<NotificationResponse>> SendVaccinationNotificationToNurses(IEnumerable<NotificationRequest> requests)
        {
            List<NotificationResponse> responses = [];
            foreach (var request in requests)
            {
                try
                {
                    var schedule = await scheduleRepository.GetVaccinationScheduleByIdAsync(request.NotificationTypeId);
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
                        Content = $"You have been assigned to the vaccination schedule: {schedule.Vaccine!.VaccineName} with {nurseRounds.Count} rounds.",
                        SendDate = DateTime.UtcNow,
                        IsRead = false,
                        Type = NotificationTypes.Vaccination,
                        SourceId = schedule.ScheduleId
                    };
                    await notificationRepository.AddAsync(notification);
                    var notiInfo = helperService.GetNotificationInformation(notification);
                    responses.Add(helperService.GetNotificationResponse(notiInfo, sender, receiver));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            if (responses.Count == 0)
            {
                return null!;
            }
            return responses;
        }

        public async Task<NotificationResponse> SendVaccinationObservationNotificationToParent(NotificationRequest requests)
        {
            var observation = await observationRepository.GetVaccinationObservationByIdAsync(requests.NotificationTypeId);
            if (observation == null)
            {
                return null!;
            }
            var result = await resultRepository.GetByIdAsync(observation.VaccinationResultId);
            if (result == null)
            {
                return null!;
            }
            var healthProfile = await profileRepository.GetHealthProfileById(result.HealthProfileId);
            var student = await studentRepository.GetStudentByHealthProfileId(healthProfile!.HealthProfileId);
            var receiver = await helperService.GetReceiverInformationAsync(requests);
            var sender = await helperService.GetSenderInformationAsync(requests);
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "Vaccination Observation Notification",
                Content = $"A post-vaccination reaction has been observed for student {student!.FullName}. " +
                      $"The reaction began at {observation.ReactionStartTime?.ToString("g") ?? "an unknown time"}, " +
                      $"identified as {observation.ReactionType ?? "unspecified"} with a severity level of {observation.SeverityLevel ?? "unspecified"}. " +
                      $"Intervention provided: {observation.Intervention ?? "none"}",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.VaccinationObservation,
                SourceId = observation.VaccinationObservationId
            };
            await notificationRepository.AddAsync(notification);
            var notiInfo = helperService.GetNotificationInformation(notification);
            return helperService.GetNotificationResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> SendVaccinationResultNotificationToParent(NotificationRequest request)
        {
            var result = await resultRepository.GetByIdAsync(request.NotificationTypeId);
            if (result == null)
            {
                return null!;
            }
            var sender = await helperService.GetSenderInformationAsync(request);
            var receiver = await helperService.GetReceiverInformationAsync(request);
            var vaccineName = result.Round!.Schedule!.Vaccine!.VaccineName ?? "Vaccination";
            var studentName = result.HealthProfile?.Student!.FullName ?? "Student";
            var startTime = result.Round!.StartTime?.ToString("d") ?? "Unknown date";


            var contentFailed = $"Your child {studentName} has not received the vaccination: {vaccineName} on {startTime}.";
            var contentSuccess = $"Your child {studentName} has received the vaccination: {vaccineName} on {startTime}.";
            var contentNotHealthQualified = $"Your child {studentName} is not qualified for the vaccination: {vaccineName} on {startTime}.";

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "Vaccination Result Notification",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.VaccinationResult,
                SourceId = result.VaccinationResultId
            };
            if (result.Status!.ToLower().Contains("completed"))
            {
                notification.Content = contentSuccess;
            }
            else if (result.Status.ToLower().Contains("failed"))
            {
                notification.Content = contentFailed;
            }
            else if (result.Status.ToLower().Contains("not qualified"))
            {
                notification.Content = contentNotHealthQualified;
            }
            await notificationRepository.AddAsync(notification);
            var notiInfo = helperService.GetNotificationInformation(notification);
            return helperService.GetNotificationResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> SendVaccinationNotificationToAdmin(NotificationRequest request)
        {
            var round = await roundRepository.GetVaccinationRoundByIdAsync(request.NotificationTypeId);
            if (round == null)
            {
                return null!;
            }
            var sender = await helperService.GetSenderInformationAsync(request);
            var receiver = await helperService.GetReceiverInformationAsync(request);
            if (sender == null || receiver == null)
            {
                return null!;
            }
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = receiver.UserId,
                SenderId = sender.UserId,
                Title = "Vaccination Round Notification",
                Content = $"A vaccination round has been completed: {round.RoundName} on {round.StartTime?.ToString("d")}.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.Vaccination,
                SourceId = round.RoundId
            };
            await notificationRepository.AddAsync(notification);
            var notiInfo = helperService.GetNotificationInformation(notification);
            return helperService.GetNotificationResponse(notiInfo, sender, receiver);
        }
    }
}