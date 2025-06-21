namespace SchoolMedicalServer.Abstractions.Dtos.MainFlow.Vaccination.Schedules
{
    public class VaccinationScheduleRequest
    {
        public Guid? VaccineId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public Guid CreatedBy { get; set; }
        public IEnumerable<VaccinationRoundRequestDto> VaccinationRounds { get; set; } = default!;
    }

    public class VaccinationRoundRequestDto
    {
        public string? RoundName { get; set; }
        public string? TargetGrade { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid NurseId { get; set; }
    }
}
