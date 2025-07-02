namespace SchoolMedicalServer.Abstractions.Entities;

public partial class Notification
{
    public Guid NotificationId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? SenderId { get; set; }
    public NotificationTypes Type { get; set; }
    public Guid SourceId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime SendDate { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime? ReadDate { get; set; }
    public virtual User? User { get; set; }
}

public enum NotificationTypes
{
    Appointment = 1,
    HealthCheckUp = 2,
    MedicalEvent = 3,
    MedicalRegistration = 4,
    Vaccination = 5,
    GeneralNotification = 6,
    VaccinationObservation = 7,
    VaccinationResult = 8,
    HealthCheckResult = 9
}
