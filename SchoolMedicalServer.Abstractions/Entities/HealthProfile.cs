using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class HealthProfile
{
    public Guid HealthProfileId { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? VaccinationResultId { get; set; }

    public Guid? HealthCheckResultId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? RecordedId { get; set; }

    public string? Notes { get; set; }

    public virtual HealthCheckResult? HealthCheckResult { get; set; }

    public virtual Student? Student { get; set; }

    public virtual VaccinationResult? VaccinationResult { get; set; }
}
