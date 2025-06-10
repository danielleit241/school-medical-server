using SchoolMedicalServer.Abstractions.Entities;

namespace SchoolMedicalServer.Abstractions.Dtos.Notification
{
    public class NotificationResponse
    {
        public NotificationResponseDto NotificationResponseDto { get; set; } = default!;
        public ReceiverInformationResponseDto ReceiverInformationDto { get; set; } = default!;
        public SenderInformationResponseDto SenderInformationDto { get; set; } = default!;
    }

    public class NotificationResponseDto
    {
        public Guid NotificationId { get; set; }

        public NotificationTypes Type { get; set; }

        public Guid SourceId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime SendDate { get; set; }
    }

    public class ReceiverInformationResponseDto
    {
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
    }

    public class SenderInformationResponseDto
    {
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
