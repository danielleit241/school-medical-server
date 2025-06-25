using SchoolMedicalServer.Abstractions.Dtos.Notification;
using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface INotificationHelperService
    {
        Task<SenderInformationResponseDto> GetSenderInformationAsync(NotificationRequest request);
        Task<ReceiverInformationResponseDto> GetReceiverInformationAsync(NotificationRequest request);
        NotificationResponseDto GetNotificationInformation(Notification notification);
        NotificationResponse GetNotificationResponse(NotificationResponseDto noti, SenderInformationResponseDto sender, ReceiverInformationResponseDto receiver);
    }
}
