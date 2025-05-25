using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class Appointment
{
    public Guid AppointmentId { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? UserId { get; set; }

    public DateOnly? AppointmentDate { get; set; }

    public TimeOnly? AppointmentTime { get; set; }

    public string? AppointmentReason { get; set; }

    public bool? ConfirmationStatus { get; set; }

    public bool? CompletionStatus { get; set; }

    public virtual Student? Student { get; set; }

    public virtual User? User { get; set; }
}
