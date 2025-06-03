using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.Dtos.Appointment;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class NotificationService(SchoolMedicalManagementContext context) : INotificationService
    {
        public async Task<NotificationResponse> SendAppoimentToNurseNotificationAsync(NotificationRequest request)
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
                Type = NotificationTypes.AppointmentReminder,
                SourceId = appointment.AppointmentId
            };
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            var notiInfo = GetNotiInfor(notification);
            return GetResponse(notiInfo, sender, receiver);
        }

        public async Task<NotificationResponse> SendAppoimentToParentNotificationAsync(NotificationRequest request)
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
                Type = NotificationTypes.AppointmentReminder,
                SourceId = appointment.AppointmentId,
            };
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();


            var notiInfo = GetNotiInfor(notification);
            return GetResponse(notiInfo, sender, receiver);
        }

        private NotificationResponseDto GetNotiInfor(Notification notification)
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

        public async Task<NotificationResponse> GetAppoimentNotificationAsync(Guid notificationId)
        {
            var notification = await context.Notifications
               .Where(n => n.NotificationId == notificationId)
               .FirstOrDefaultAsync();

            if (notification == null)
            {
                return null!;
            }

            var sender = context.Users
                .Where(u => u.UserId == notification.SenderId)
                .Select(u => new SenderInformationResponseDto
                {
                    UserId = u.UserId,
                    UserName = u.FullName
                })
                .FirstOrDefault();

            var receiver = context.Users
                .Where(u => u.UserId == notification.ReceiverId)
                .Select(u => new ReceiverInformationResponseDto
                {
                    UserId = u.UserId,
                    UserName = u.FullName
                })
                .FirstOrDefault();

            var notiInfo = GetNotiInfor(notification);
            return GetResponse(notiInfo, sender!, receiver!);
        }



        public async Task<PaginationResponse<NotificationResponse>> GetAppoimentNotificationsByUserAsync(PaginationRequest pagination, Guid userId)
        {
            var totalCount = await context.Notifications
                .Where(n => n.ReceiverId == userId && n.Type == NotificationTypes.AppointmentReminder)
                .CountAsync();

            if(totalCount == 0)
            {
                return null!;
            }

            var notifications = await context.Notifications
                .Where(n => n.ReceiverId == userId && n.Type == NotificationTypes.AppointmentReminder)
                .OrderByDescending(n => n.SendDate)
                 .Skip((pagination!.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var result = new List<NotificationResponse>();

            foreach (var notification in notifications)
            {
                var sender = context.Users
                    .Where(u => u.UserId == notification.SenderId)
                    .Select(u => new SenderInformationResponseDto
                    {
                        UserId = u.UserId,
                        UserName = u.FullName
                    })
                    .FirstOrDefault();

                var receiver = context.Users
                    .Where(u => u.UserId == notification.ReceiverId)
                    .Select(u => new ReceiverInformationResponseDto
                    {
                        UserId = u.UserId,
                        UserName = u.FullName
                    })
                    .FirstOrDefault();

                var notiInfo = GetNotiInfor(notification);
                var response = GetResponse(notiInfo, sender!, receiver!);

                result.Add(response);
            }
            return new PaginationResponse<NotificationResponse>(
                   pagination.PageIndex,
                   pagination.PageSize,
                    totalCount,
                    result
             );
        }
    }
}
