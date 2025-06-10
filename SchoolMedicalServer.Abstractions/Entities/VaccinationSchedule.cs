namespace SchoolMedicalServer.Abstractions.Entities;

public partial class VaccinationSchedule
{
    public Guid ScheduleId { get; set; }
    public Guid? StudentId { get; set; }
    public Guid? VaccineId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    public DateOnly? ParentNotificationStartDate { get; set; }
    public bool ParentConfirmed { get; set; }
    public DateOnly? ParentNotificationEndDate { get; set; }

    public virtual Student? Student { get; set; }
    public virtual ICollection<VaccinationResult> VaccinationResults { get; set; } = new List<VaccinationResult>();
    public virtual VaccinationDetail? Vaccine { get; set; }
    public virtual ICollection<VaccinationRound> Rounds { get; set; } = new List<VaccinationRound>();
}
