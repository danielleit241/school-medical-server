using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Infrastructure.Services
{
    public class NotificationHelperService(IUserRepository userRepository) : INotificationHelperService
    {
        public NotificationResponseDto GetNotificationInformation(Notification notification)
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

        public async Task<SenderInformationResponseDto> GetSenderInformationAsync(NotificationRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.SenderId);
            return new SenderInformationResponseDto
            {
                UserId = user?.UserId,
                UserName = user?.FullName
            };
        }

        public async Task<ReceiverInformationResponseDto> GetReceiverInformationAsync(NotificationRequest request)
        {
            var user = await userRepository.GetByIdAsync(request.ReceiverId);
            return new ReceiverInformationResponseDto
            {
                UserId = user?.UserId,
                UserName = user?.FullName
            };
        }

        public NotificationResponse GetNotificationResponse(NotificationResponseDto noti, SenderInformationResponseDto sender, ReceiverInformationResponseDto receiver)
        {
            return new NotificationResponse
            {
                NotificationResponseDto = noti,
                SenderInformationDto = sender!,
                ReceiverInformationDto = receiver!
            };
        }
    }
}