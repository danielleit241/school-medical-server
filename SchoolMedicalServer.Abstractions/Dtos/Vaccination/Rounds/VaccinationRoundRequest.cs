namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination.Rounds
{
    public class VaccinationRoundRequest
    {
        public Guid? ScheduleId { get; set; }
        public string? RoundName { get; set; }
        public string? TargetGrade { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid NurseId { get; set; }
    }
}
