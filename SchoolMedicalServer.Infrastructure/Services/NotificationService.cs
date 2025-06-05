using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class NotificationService(SchoolMedicalManagementContext context) : INotificationService
    {
        public async Task<PaginationResponse<NotificationResponse>> GetUserNotificationsAsync(PaginationRequest? pagination, Guid userId)
        {
            var totalCount = await context.Notifications
                .Where(n => n.ReceiverId == userId)
                .CountAsync();

            if (totalCount == 0)
            {
                return null!;
            }

            var notifications = await context.Notifications
                .Where(n => n.ReceiverId == userId)
                .OrderByDescending(n => n.SendDate)
                 .Skip((pagination!.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var result = new List<NotificationResponse>();

            foreach (var notification in notifications)
            {
                var request = new NotificationRequest
                {
                    SenderId = notification.SenderId,
                    ReceiverId = notification.ReceiverId
                };
                var notiInfo = NotificationInformation(notification);
                var sender = SenderInformation(request)!;
                var receiver = ReceiverInformation(request)!;

                result.Add(GetResponse(notiInfo, sender, receiver));
            }

            return new PaginationResponse<NotificationResponse>(
                   pagination.PageIndex,
                   pagination.PageSize,
                    totalCount,
                    result
             );
        }

        public async Task<NotificationResponse> SendAppoimentNotificationToNurseAsync(NotificationRequest request)
        {
            var appointment = await context.Appointments.Include(s => s.Student).Where(a => a.AppointmentId == request.NotificationTypeId).FirstOrDefaultAsync();
            if (appointment == null)
            {
                return null!;
            }

            var receiver = ReceiverInformation(request);
            var sender = SenderInformation(request);

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                ReceiverId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "New Appointment Notification",
                Content = $"You have a new appointment scheduled with Parent of {appointment.Student?.FullName} on {appointment.AppointmentDate?.ToString("d")} from {appointment.AppointmentStartTime?.ToString()} to {appointment.AppointmentEndTime?.ToString()}.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.Appointment,
                SourceId = appointment.AppointmentId
            };
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> SendAppoimentNotificationToParentAsync(NotificationRequest request)
        {
            var appointment = await context.Appointments.Where(a => a.AppointmentId == request.NotificationTypeId)
                .Include(a => a.Student)
                .FirstOrDefaultAsync();

            if (appointment == null)
                return null!;

            var receiver = ReceiverInformation(request);
            var sender = SenderInformation(request);

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                SenderId = sender!.UserId,
                ReceiverId = receiver!.UserId,
                Title = "Appointment Confirmation",
                Content = $"Your appointment with {sender!.UserName} is confirmed for {appointment.AppointmentDate?.ToString("d")} from {appointment.AppointmentStartTime?.ToString()} to {appointment.AppointmentEndTime?.ToString()}.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.Appointment,
                SourceId = appointment.AppointmentId,
            };
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();


            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> GetAppoimentNotificationAsync(Guid notificationId)
        {
            var notification = await context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId && n.Type == NotificationTypes.Appointment);

            if (notification == null)
            {
                return null!;
            }

            var request = new NotificationRequest
            {
                SenderId = notification.SenderId,
                ReceiverId = notification.ReceiverId
            };
            var notiInfo = NotificationInformation(notification);
            var sender = SenderInformation(request)!;
            var receiver = ReceiverInformation(request)!;

            return GetResponse(notiInfo, sender, receiver);
        }

        //public async Task<PaginationResponse<NotificationResponse>> GetAppoimentNotificationsByUserAsync(PaginationRequest pagination, Guid userId)
        //{
        //    var totalCount = await context.Notifications
        //        .Where(n => n.ReceiverId == userId && n.Type == NotificationTypes.AppointmentReminder)
        //        .CountAsync();

        //    if (totalCount == 0)
        //    {
        //        return null!;
        //    }

        //    var notifications = await context.Notifications
        //        .Where(n => n.ReceiverId == userId && n.Type == NotificationTypes.AppointmentReminder)
        //        .OrderByDescending(n => n.SendDate)
        //         .Skip((pagination!.PageIndex - 1) * pagination.PageSize)
        //        .Take(pagination.PageSize)
        //        .ToListAsync();

        //    var result = new List<NotificationResponse>();

        //    foreach (var notification in notifications)
        //    {
        //        var request = new NotificationRequest
        //        {
        //            SenderId = notification.SenderId,
        //            ReceiverId = notification.ReceiverId
        //        };
        //        var notiInfo = NotificationInformation(notification);
        //        var sender = SenderInformation(request)!;
        //        var receiver = ReceiverInformation(request)!;

        //        result.Add(GetResponse(notiInfo, sender, receiver));
        //    }

        //    return new PaginationResponse<NotificationResponse>(
        //           pagination.PageIndex,
        //           pagination.PageSize,
        //            totalCount,
        //            result
        //     );
        //}

        public async Task<NotificationResponse> SendMedicalRegistrationApprovedNotificationToParentAsync(NotificationRequest request)
        {
            var medicalRegistration = await context.MedicalRegistrations
                .Include(mr => mr.Student)
                .FirstOrDefaultAsync(mr => mr.RegistrationId == request.NotificationTypeId && mr.Status == true);

            if (medicalRegistration == null)
            {
                return null!;
            }

            var receiver = ReceiverInformation(request);
            var sender = SenderInformation(request);

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                ReceiverId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "Medical Registration Approved",
                Content = $"Your child's medication registration ({medicalRegistration.MedicationName}) has been approved by the nurse.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.MedicalRegistration,
                SourceId = medicalRegistration.RegistrationId
            };
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> SendMedicalRegistrationCompletedNotificationToParentAsync(NotificationRequest request)
        {
            var medicalRegistrationDetail = await context.MedicalRegistrationDetails
                   .Include(mrd => mrd.MedicalRegistration)
                   .ThenInclude(mr => mr.Student)
                   .FirstOrDefaultAsync(mrd => mrd.MedicalRegistrationDetailsId == request.NotificationTypeId);

            if (medicalRegistrationDetail == null)
            {
                return null!;
            }

            var receiver = ReceiverInformation(request);
            var sender = SenderInformation(request);

            var medicationName = medicalRegistrationDetail.MedicalRegistration?.MedicationName ?? "The medication";
            var doseNumber = !string.IsNullOrEmpty(medicalRegistrationDetail.DoseNumber) ? $" (Dose {medicalRegistrationDetail.DoseNumber})" : "";
            var doseTime = !string.IsNullOrEmpty(medicalRegistrationDetail.DoseTime) ? $" at {medicalRegistrationDetail.DoseTime}" : "";
            var studentName = medicalRegistrationDetail.MedicalRegistration?.Student?.FullName ?? "your child";

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                ReceiverId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "Medication Dose Completed",
                Content = $"A dose{doseNumber} of medication ({medicationName}) for {studentName}{doseTime} has been completed by the nurse.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.MedicalRegistration,
                SourceId = medicalRegistrationDetail.MedicalRegistrationDetailsId
            };
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> GetMedicalRegistrationNotificationAsync(Guid notificationId)
        {
            var notification = await context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);

            if (notification == null)
            {
                return null!;
            }

            var request = new NotificationRequest
            {
                SenderId = notification.SenderId,
                ReceiverId = notification.ReceiverId
            };
            var notiInfo = NotificationInformation(notification);
            var sender = SenderInformation(request)!;
            var receiver = ReceiverInformation(request)!;

            return GetResponse(notiInfo, sender, receiver);

        }

        //public async Task<PaginationResponse<NotificationResponse>> GetMedicalRegistrationNotificationsAsync(PaginationRequest pagination, Guid userId)
        //{
        //    var totalCount = await context.Notifications
        //          .Where(n => n.ReceiverId == userId && n.Type == NotificationTypes.MedicalRegistration)
        //          .CountAsync();

        //    if (totalCount == 0)
        //    {
        //        return null!;
        //    }

        //    var notifications = await context.Notifications
        //        .Where(n => n.ReceiverId == userId && n.Type == NotificationTypes.MedicalRegistration)
        //        .OrderByDescending(n => n.SendDate)
        //        .Skip((pagination!.PageIndex - 1) * pagination.PageSize)
        //        .Take(pagination.PageSize)
        //        .ToListAsync();

        //    var result = new List<NotificationResponse>();
        //    foreach (var notification in notifications)
        //    {
        //        var request = new NotificationRequest
        //        {
        //            SenderId = notification.SenderId,
        //            ReceiverId = notification.ReceiverId
        //        };
        //        var notiInfo = NotificationInformation(notification);
        //        var sender = SenderInformation(request)!;
        //        var receiver = ReceiverInformation(request)!;

        //        result.Add(GetResponse(notiInfo, sender, receiver));
        //    }

        //    return new PaginationResponse<NotificationResponse>(
        //        pagination.PageIndex,
        //        pagination.PageSize,
        //        totalCount,
        //        result
        //    );
        //}
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

        private SenderInformationResponseDto? SenderInformation(NotificationRequest request)
        {
            var sender = context.Users
                .Where(u => u.UserId == request.SenderId)
                .Select(u => new SenderInformationResponseDto
                {
                    UserId = u.UserId,
                    UserName = u.FullName
                })
                .FirstOrDefault();
            return sender;
        }

        private ReceiverInformationResponseDto? ReceiverInformation(NotificationRequest request)
        {
            var receiver = context.Users
                .Where(u => u.UserId == request.ReceiverId)
                .Select(u => new ReceiverInformationResponseDto
                {
                    UserId = u.UserId,
                    UserName = u.FullName
                })
                .FirstOrDefault();
            return receiver;
        }

        private NotificationResponse GetResponse(NotificationResponseDto noti, SenderInformationResponseDto sender, ReceiverInformationResponseDto receiver)
        {
            return new NotificationResponse
            {
                NotificationResponseDto = noti,
                SenderInformationDto = sender,
                ReceiverInformationDto = receiver
            };
        }

        public async Task<NotificationResponse> SendMedicalEventNotificationToParentAsync(NotificationRequest request)
        {
            var medicalEvent = await context.MedicalEvents
                .Include(me => me.Student)
                .FirstOrDefaultAsync(me => me.EventId == request.NotificationTypeId);
            if (medicalEvent == null)
            {
                return null!;
            }
            var receiver = ReceiverInformation(request);
            var sender = SenderInformation(request);
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                ReceiverId = receiver!.UserId,
                SenderId = sender!.UserId,
                Title = "Medical Event Notification",
                Content = $"A medical event has been recorded for {medicalEvent.Student?.FullName} on {medicalEvent.EventDate?.ToString("d")}.",
                SendDate = DateTime.UtcNow,
                IsRead = false,
                Type = NotificationTypes.MedicalEvent,
                SourceId = medicalEvent.EventId
            };
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            var notiInfo = NotificationInformation(notification);
            return GetResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> GetMedicalEventNotificationAsync(Guid notificationTypeId)
        {
            var notification = await context.Notifications
                 .FirstOrDefaultAsync(n => n.NotificationId == notificationTypeId && n.Type == NotificationTypes.MedicalEvent);
            if (notification == null)
            {
                return null!;
            }
            var request = new NotificationRequest
            {
                SenderId = notification.SenderId,
                ReceiverId = notification.ReceiverId
            };
            var notiInfo = NotificationInformation(notification);
            var sender = SenderInformation(request)!;
            var receiver = ReceiverInformation(request)!;
            return GetResponse(notiInfo, sender, receiver);
        }

        public async Task<bool> ReadAllNotificationsAsync(Guid userId)
        {
            var notifications = await context.Notifications
                .Where(n => n.ReceiverId == userId && !n.IsRead)
                .ToListAsync();
            if (notifications.Count == 0)
            {
                return false;
            }
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUserUnReadNotificationsAsync(Guid? userId)
        {
            var unreadNotis = await context.Notifications
                .Where(n => n.ReceiverId == userId && !n.IsRead)
                .CountAsync();
            return unreadNotis;
        }
    }
}
