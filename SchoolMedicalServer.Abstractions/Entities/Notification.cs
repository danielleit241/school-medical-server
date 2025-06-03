namespace SchoolMedicalServer.Abstractions.Entities;

public partial class Notification
{
    public Guid NotificationId { get; set; }

    public Guid? ReceiverId { get; set; }

    public Guid? SenderId { get; set; }

    public NotificationTypes Type { get; set; }

    public Guid SourceId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime SendDate { get; set; }

    public bool IsRead { get; set; } = false;

    public bool IsConfirmed { get; set; } = false;

    public DateTime? ConfirmedAt { get; set; }
}

public enum NotificationTypes
{
    AppointmentReminder = 1,
    HealthCheckReminder = 2,
    MedicalEventUpdate = 3,
    MedicalRegistration = 4,
    VaccinationSchedule = 5,
    GeneralNotification = 6
}
