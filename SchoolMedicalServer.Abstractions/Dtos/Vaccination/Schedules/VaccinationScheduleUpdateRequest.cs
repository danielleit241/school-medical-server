namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules
{
    public class VaccinationScheduleUpdateRequest
    {
        public Guid? VaccineId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public Guid CreatedBy { get; set; }
        public IEnumerable<VaccinationRoundRequestUpdateDto> VaccinationRounds { get; set; } = default!;
    }

    public class VaccinationRoundRequestUpdateDto : VaccinationRoundRequestDto
    {
        public Guid? RoundId { get; set; }
    }
}
