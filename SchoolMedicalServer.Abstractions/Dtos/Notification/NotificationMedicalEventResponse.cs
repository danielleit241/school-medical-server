namespace SchoolMedicalServer.Abstractions.Dtos.Notification
{
    public class NotificationMedicalEventResponse
    {
        public NotificationRequest? ToParent { get; set; }
        public NotificationRequest? ToManager { get; set; }
    }
}
