using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class AppointmentNotificationService(
        INotificationRepository notificationRepository,
        IAppointmentRepository appointmentRepository,
        INotificationHelperService notificationHelper) : IAppointmentNotificationService
    {
        public async Task<NotificationResponse> SendAppointmentNotificationToNurseAsync(NotificationRequest request)
        {
            var appointment = await appointmentRepository.GetByIdWithStudentAsync(request.NotificationTypeId);
            if (appointment == null)
            {
                return null!;
            }

            var receiver = await notificationHelper.GetReceiverInformationAsync(request);
            var sender = await notificationHelper.GetSenderInformationAsync(request);

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
            var notiInfo = notificationHelper.GetNotificationInformation(notification);
            return notificationHelper.GetNotificationResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> SendAppointmentNotificationToParentAsync(NotificationRequest request)
        {
            var appointment = await appointmentRepository.GetByIdWithStudentAsync(request.NotificationTypeId);

            if (appointment == null)
                return null!;

            var receiver = await notificationHelper.GetReceiverInformationAsync(request);
            var sender = await notificationHelper.GetSenderInformationAsync(request);

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
            var notiInfo = notificationHelper.GetNotificationInformation(notification);
            return notificationHelper.GetNotificationResponse(notiInfo, sender, receiver);
        }
    }
}