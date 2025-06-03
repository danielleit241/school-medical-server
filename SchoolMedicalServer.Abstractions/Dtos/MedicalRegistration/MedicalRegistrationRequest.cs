using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration
{
    public class MedicalRegistrationRequest
    {
        public MedicalRegistrationRequestDto MedicalRegistration { get; set; } = default!;
        public List<MedicalRegistrationDetailsRequest> MedicalRegistrationDetails { get; set; } = new List<MedicalRegistrationDetailsRequest>();
    }

    public class MedicalRegistrationRequestDto
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
        public string TotalDosages { get; set; } = default!;

        public string Notes { get; set; } = default!;

        [Required(ErrorMessage = "Parent consent is required.")]
        public bool ParentConsent { get; set; }
    }

    public class MedicalRegistrationDetailsRequest
    {
        [Required(ErrorMessage = "Dose number is required.")]
        public string? DoseNumber { get; set; }

        [Required(ErrorMessage = "Dose time is required.")]
        public string? DoseTime { get; set; }

        public string? Notes { get; set; }
    }
}
