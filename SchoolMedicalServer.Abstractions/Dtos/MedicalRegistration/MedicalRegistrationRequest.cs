using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration
{
    public class MedicalRegistrationRequest
    {
        [Required(ErrorMessage = "Student ID is required.")]
        public Guid StudentId { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Date submitted is required.")]
        public DateOnly? DateSubmitted { get; set; }

        [Required(ErrorMessage = "Medication name is required.")]
        public string MedicationName { get; set; } = default!;

        [Required(ErrorMessage = "Dosage is required.")]
        public string Dosage { get; set; } = default!;

        public string Notes { get; set; } = default!;

        [Required(ErrorMessage = "Parent consent is required.")]
        public bool ParentConsent { get; set; }
    }
}
