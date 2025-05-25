using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class Notification
{
    public Guid NotificationId { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? VaccineScheduleId { get; set; }

    public Guid? HealthCheckScheduleId { get; set; }

    public Guid? EventId { get; set; }

    public DateTime? SendDate { get; set; }

    public bool? Status { get; set; }

    public virtual Student? Student { get; set; }

    public virtual User? User { get; set; }
}
