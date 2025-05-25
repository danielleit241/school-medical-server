using System;
using System.Collections.Generic;

namespace SchoolMedicalServer.Abstractions.Entities;

public partial class VaccinationSchedule
{
    public Guid ScheduleId { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? VaccineId { get; set; }

    public string? Title { get; set; }

    public string? Round { get; set; }

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? TargetGrade { get; set; }

    public virtual Student? Student { get; set; }

    public virtual ICollection<VaccinationResult> VaccinationResults { get; set; } = new List<VaccinationResult>();

    public virtual VaccineDetail? Vaccine { get; set; }
}
