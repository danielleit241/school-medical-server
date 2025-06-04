using System.Text.Json.Serialization;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class Appointment
{
    public Guid AppointmentId { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? StaffNurseId { get; set; }

    public string? Topic { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public TimeOnly? AppointmentStartTime { get; set; }

    public TimeOnly? AppointmentEndTime { get; set; }

    public string? AppointmentReason { get; set; }

    public bool? ConfirmationStatus { get; set; }

    public bool? CompletionStatus { get; set; }

    [JsonIgnore]
    public virtual Student? Student { get; set; }
    [JsonIgnore]
    public virtual User? User { get; set; }
}
