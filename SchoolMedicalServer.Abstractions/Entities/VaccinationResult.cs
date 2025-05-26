using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class VaccinationResult
{
    public Guid VaccinationResultId { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? ScheduleId { get; set; }

    public int? DoseNumber { get; set; }

    public DateOnly? VaccinationDate { get; set; }

    public string? InjectionSite { get; set; }

    public string? ImmediateReaction { get; set; }

    public DateTime? ReactionStartTime { get; set; }

    public string? ReactionType { get; set; }

    public string? SeverityLevel { get; set; }

    public string? Notes { get; set; }

    public Guid? RecordedId { get; set; }

    public virtual ICollection<HealthProfile> HealthProfiles { get; set; } = new List<HealthProfile>();

    public virtual VaccinationSchedule? Schedule { get; set; }

    public virtual Student? Student { get; set; }
}
