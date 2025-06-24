using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Rounds
{
    public class VaccinationRoundRequest
    {
        [Required]
        public Guid? ScheduleId { get; set; }
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
