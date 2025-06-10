namespace SchoolMedicalServer.Abstractions.Dtos.Notification
{
    public class NotificationRequest
    {
        public Guid NotificationTypeId { get; set; }
        public Guid? SenderId { get; set; }
        public Guid? ReceiverId { get; set; }
    }
}
