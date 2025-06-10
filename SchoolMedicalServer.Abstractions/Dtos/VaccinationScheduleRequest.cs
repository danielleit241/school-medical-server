namespace SchoolMedicalServer.Abstractions.Dtos
{
    public class VaccinationScheduleRequest
    {
        public Guid? VaccineId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IEnumerable<VaccinationRoundRequestDto> VaccinationRounds { get; set; } = default!;
    }

    public class VaccinationRoundRequestDto
    {
        public string? RoundName { get; set; }
        public string? TargetGrade { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
