using SchoolMedicalServer.Abstractions.Dtos.Vaccination.Vaccines;

namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination.Schedules
{
    public class VaccinationScheduleResponse
    {
        public VaccinationScheduleResponseDto VaccinationScheduleResponseDto { get; set; } = default!;
        public VaccinationDetailsResponse VaccinationDetailsResponse { get; set; } = default!;
        public IEnumerable<VaccinationRoundResponseDto> VaccinationRounds { get; set; } = default!;
    }

    public class VaccinationScheduleResponseDto
    {
        public Guid ScheduleId { get; set; }
        public Guid? VaccineId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? ParentNotificationStartDate { get; set; }
        public DateOnly? ParentNotificationEndDate { get; set; }

        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class VaccinationRoundResponseDto
    {
        public Guid RoundId { get; set; }
        public string? RoundName { get; set; }
        public string? TargetGrade { get; set; }
        public string? Description { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool Status { get; set; }
    }
}
