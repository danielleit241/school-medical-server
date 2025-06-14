using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Vaccines;

namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules
{
    public class VaccinationScheduleDetailsResponse
    {
        public VaccinationDetailsResponse VaccinationDetailsResponse { get; set; } = default!;
        public IEnumerable<VaccinationRoundResponseDto> VaccinationRounds { get; set; } = default!;
    }
    public class VaccinationRoundResponseDto
    {
        public Guid RoundId { get; set; }
        public string? RoundName { get; set; }
        public string? TargetGrade { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid NurseId { get; set; }
        public bool Status { get; set; }
    }
}
