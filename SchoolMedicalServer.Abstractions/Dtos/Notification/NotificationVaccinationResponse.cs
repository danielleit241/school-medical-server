namespace SchoolMedicalServer.Abstractions.Dtos.Notification
{
    public class NotificationVaccinationResponse(List<NotificationRequest> toParents, List<NotificationRequest> toNurses)
    {
        public List<NotificationRequest> ToParent { get; set; } = toParents ?? [];
        public List<NotificationRequest> ToNurse { get; set; } = toNurses ?? [];
    }
}
