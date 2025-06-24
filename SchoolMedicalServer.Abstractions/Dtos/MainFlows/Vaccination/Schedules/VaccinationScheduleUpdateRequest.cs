using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.Vaccination.Schedules
{
    public class VaccinationScheduleUpdateRequest
    {
        [Required]
        public Guid? ScheduleId { get; set; }
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
    }
}
