using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class NotificationService(
        IBaseRepository baseRepository,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IAppointmentRepository appointmentRepository,
        IMedicalRegistrationRepository medicalRegistrationRepository,
        IMedicalRegistrationDetailsRepository medicalRegistrationDetailsRepository,
        IMedicalEventRepository medicalEventRepository,
        IStudentRepository studentRepository) : INotificationService
    {
        public async Task<PaginationResponse<NotificationResponse>> GetUserNotificationsAsync(PaginationRequest? pagination, Guid userId)
        {
            var totalCount = await notificationRepository.CountByUserIdAsync(userId);

            if (totalCount == 0)
            {
                return null!;
            }

            var notifications = await notificationRepository.GetByUserIdPagedAsync(userId, pagination?.PageIndex ?? 0, pagination?.PageSize ?? 10);

            var result = new List<NotificationResponse>();

            foreach (var notification in notifications)
            {
                var request = new NotificationRequest
                {
                    SenderId = notification.SenderId,
                    ReceiverId = notification.UserId
                };
                var notiInfo = NotificationInformation(notification);
                var sender = await SenderInformation(request)!;
                var receiver = await ReceiverInformation(request)!;

                result.Add(GetResponse(notiInfo, sender, receiver));
            }

            return new PaginationResponse<NotificationResponse>(
                   pagination!.PageIndex,
                   pagination.PageSize,
                    totalCount,
                    result
             );
        }

        public async Task<NotificationResponse> SendAppoimentNotificationToNurseAsync(NotificationRequest request)
        {
            var appointment = await appointmentRepository.GetByIdWithStudentAsync(request.NotificationTypeId);
            if (appointment == null)
            {
                return null!;
            }

            var receiver = await ReceiverInformation(request);
            var sender = await SenderInformation(request);

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "New Appointment Notification",
                Content = $"You have a new appointment scheduled with Parent of {appointment.Student?.FullName} on {appointment.AppointmentDate?.ToString("d")} from {appointment.AppointmentStartTime?.ToString()} to {appointment.AppointmentEndTime?.ToString()}.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.Appointment,
                SourceId = appointment.AppointmentId
            };
            await notificationRepository.AddAsync(notification);
            await baseRepository.SaveChangesAsync();

            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> SendAppoimentNotificationToParentAsync(NotificationRequest request)
        {
            var appointment = await appointmentRepository.GetByIdWithStudentAsync(request.NotificationTypeId);

            if (appointment == null)
                return null!;

            var receiver = await ReceiverInformation(request);
            var sender = await SenderInformation(request);

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                SenderId = sender!.UserId,
                UserId = receiver!.UserId,
                Title = appointment.CompletionStatus == false ? "Appointment Confirmation" : "Appointment Completion",
                Content = appointment.CompletionStatus == true
                            ? $"Your appointment with {sender!.UserName} is completed for {appointment.AppointmentDate?.ToString("d")} from {appointment.AppointmentStartTime?.ToString()} to {appointment.AppointmentEndTime?.ToString()}."
                            : $"Your appointment with {sender!.UserName} is confirmed for {appointment.AppointmentDate?.ToString("d")} from {appointment.AppointmentStartTime?.ToString()} to {appointment.AppointmentEndTime?.ToString()}.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.Appointment,
                SourceId = appointment.AppointmentId,
            };
            await notificationRepository.AddAsync(notification);
            await baseRepository.SaveChangesAsync();


            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }
        public async Task<NotificationResponse> SendMedicalRegistrationNotificationToNurseAsync(NotificationRequest request)
        {
            var medicalRegistration = await medicalRegistrationRepository.GetByIdAsync(request.NotificationTypeId);
            if (medicalRegistration == null)
            {
                return null!;
            }
            var receiver = await ReceiverInformation(request);
            var sender = await SenderInformation(request);
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
            await baseRepository.SaveChangesAsync();
            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }
        public async Task<NotificationResponse> SendMedicalRegistrationApprovedNotificationToParentAsync(NotificationRequest request)
        {
            var medicalRegistration = await medicalRegistrationRepository.GetApprovedByIdWithStudentAsync(request.NotificationTypeId);

            if (medicalRegistration == null)
            {
                return null!;
            }

            var receiver = await ReceiverInformation(request);
            var sender = await SenderInformation(request);

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
            await baseRepository.SaveChangesAsync();

            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }
        public async Task<NotificationResponse> SendMedicalRegistrationCompletedNotificationToParentAsync(NotificationRequest request)
        {
            var medicalRegistrationDetail = await medicalRegistrationDetailsRepository.GetByIdWithRegistrationAndStudentAsync(request.NotificationTypeId);

            if (medicalRegistrationDetail == null)
            {
                return null!;
            }

            var receiver = await ReceiverInformation(request);
            var sender = await SenderInformation(request);

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
            await baseRepository.SaveChangesAsync();

            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }

        private NotificationResponseDto NotificationInformation(Notification notification)
        {
            return new NotificationResponseDto
            {
                NotificationId = notification.NotificationId,
                Type = notification.Type,
                SourceId = notification.SourceId,
                Title = notification.Title,
                Content = notification.Content,
                SendDate = notification.SendDate
            };
        }

        private async Task<SenderInformationResponseDto> SenderInformation(NotificationRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.SenderId);
            var sender = new SenderInformationResponseDto
            {
                UserId = user?.UserId,
                UserName = user?.FullName
            };
            return sender;
        }

        private async Task<ReceiverInformationResponseDto> ReceiverInformation(NotificationRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.ReceiverId);
            var receiver = new ReceiverInformationResponseDto
            {
                UserId = user?.UserId,
                UserName = user?.FullName
            };
            return receiver;
        }

        private NotificationResponse GetResponse(NotificationResponseDto noti, SenderInformationResponseDto sender, ReceiverInformationResponseDto receiver)
        {
            return new NotificationResponse
            {
                NotificationResponseDto = noti,
                SenderInformationDto = sender!,
                ReceiverInformationDto = receiver!
            };
        }

        public async Task<NotificationResponse> SendMedicalEventNotificationToParentAsync(NotificationRequest request)
        {
            var medicalEvent = await medicalEventRepository.GetByIdWithStudentAsync(request.NotificationTypeId);
            if (medicalEvent == null)
            {
                return null!;
            }
            var receiver = await ReceiverInformation(request);
            var sender = await SenderInformation(request);
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
            await baseRepository.SaveChangesAsync();

            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }

        public async Task<bool> ReadAllNotificationsAsync(Guid userId)
        {
            var notifications = await notificationRepository.GetUnreadByUserIdAsync(userId);
            if (notifications.Count == 0)
            {
                return false;
            }
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await baseRepository.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUserUnReadNotificationsAsync(Guid? userId)
        {
            var unreadNotis = await notificationRepository.CountUnreadByUserIdAsync(userId ?? Guid.Empty);
            return unreadNotis;
        }

        public async Task<NotificationResponse> GetUserNotificationDetailsAsync(Guid notificationId)
        {
            var noti = await notificationRepository.GetByIdAsync(notificationId);
            if (noti == null)
            {
                return null!;
            }
            var request = new NotificationRequest
            {
                SenderId = noti.SenderId,
                ReceiverId = noti.UserId
            };
            var notiInfo = NotificationInformation(noti);
            var sender = await SenderInformation(request);
            var receiver = await ReceiverInformation(request);
            return GetResponse(notiInfo, sender, receiver);
        }
    }
}
