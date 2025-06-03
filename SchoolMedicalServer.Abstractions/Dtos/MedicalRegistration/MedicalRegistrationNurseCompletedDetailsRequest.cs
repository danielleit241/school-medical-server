using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration
{
    public class MedicalRegistrationNurseCompletedDetailsRequest
    {
        [Required(ErrorMessage = "Staff nurse id is required.")]
        public Guid? StaffNurseId { get; set; }

        [Required(ErrorMessage = "Dose number is required.")]
        public string? DoseNumber { get; set; }

        [Required(ErrorMessage = "Date approved is required.")]
        public DateOnly? DateCompleted { get; set; }

        public string? Notes { get; set; }
    }
}
