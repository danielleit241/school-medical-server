namespace SchoolMedicalServer.Abstractions.Entities
{
    public class VaccinationRound
    {
        public Guid RoundId { get; set; }
        public Guid ScheduleId { get; set; }
        public string? RoundName { get; set; }
        public string? TargetGrade { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool Status { get; set; }
        public Guid NurseId { get; set; }


        public virtual VaccinationSchedule? Schedule { get; set; }
        public virtual ICollection<VaccinationResult> VaccinationResults { get; set; } = [];
    }
}