using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class MedicalEvent
{
    public Guid EventId { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? EventDateTime { get; set; }

    public string? EventType { get; set; }

    public string? EventDescription { get; set; }

    public string? Location { get; set; }

    public Guid? RecordedId { get; set; }

    public string? SeverityLevel { get; set; }

    public bool? ParentNotified { get; set; }

    public virtual ICollection<MedicalRequest> MedicalRequests { get; set; } = new List<MedicalRequest>();

    public virtual Student? Student { get; set; }

    public virtual User? User { get; set; }
}
