using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Appointment
{
    public class AppointmentRequest
    {
        [Required]
        public Guid? StudentId { get; set; }

        [Required]
        public Guid? UserId { get; set; }

        [Required]
        public Guid? StaffNurseId { get; set; }

        [Required]
        public string? Topic { get; set; }

        [Required]
        public DateOnly? AppointmentDate { get; set; }

        [Required]
        public TimeOnly? AppointmentStartTime { get; set; }

        [Required]
        public TimeOnly? AppointmentEndTime { get; set; }

        [Required]
        public string? AppointmentReason { get; set; }
    }
}
