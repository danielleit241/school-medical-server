using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Schedules
{
    public class VaccinationScheduleRequest
    {
        [Required]
        public Guid? VaccineId { get; set; }
        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }
        [Required]
        public DateOnly? StartDate { get; set; }
        [Required]
        public DateOnly? EndDate { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        public IEnumerable<VaccinationRoundRequestDto> VaccinationRounds { get; set; } = default!;
    }

    public class VaccinationRoundRequestDto
    {
        [Required]
        public string? RoundName { get; set; }
        [Required]
        public string? TargetGrade { get; set; }

        public string? Description { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
        [Required]
        public Guid NurseId { get; set; }
    }
}
