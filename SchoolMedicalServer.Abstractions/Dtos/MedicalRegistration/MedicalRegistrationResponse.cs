using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration
{
    public class MedicalRegistrationResponse
    {
        public MedicalRegistrationResponseDto? MedicalRegistration { get; set; }
        public List<MedicalRegistrationDetailsResponse>? MedicalRegistrationDetails { get; set; } = new List<MedicalRegistrationDetailsResponse>();
        public MedicalRegistrationNurseApprovedResponse? NurseApproved { get; set; }
        public MedicalRegistrationStudentResponse? Student { get; set; }
        public MedicalRegistrationParentResponse? Parent { get; set; }
    }

    public class MedicalRegistrationResponseDto
    {
        public Guid RegistrationId { get; set; }
        public string? MedicationName { get; set; }
        public string? TotalDosages { get; set; }
        public string? Notes { get; set; }
        public bool ParentConsent { get; set; }
        public DateOnly? DateSubmitted { get; set; }
        public string? PictureUrl { get; set; }
        public bool? Status { get; set; }
        public string? NurseNotes { get; set; }
    }

    public class MedicalRegistrationDetailsResponse
    {
        [Required(ErrorMessage = "Dose number is required.")]
        public string? DoseNumber { get; set; }

        [Required(ErrorMessage = "Dose time is required.")]
        public string? DoseTime { get; set; }

        public string? Notes { get; set; }

        public bool? IsCompleted { get; set; } = false;

        public DateTime? DateCompleted { get; set; }
    }

    public class MedicalRegistrationNurseApprovedResponse
    {
        public Guid? StaffNurseId { get; set; }
        public string? StaffNurseFullName { get; set; }
        public string? StaffNurseNotes { get; set; }
        public DateOnly? DateApproved { get; set; }
    }

    public class MedicalRegistrationStudentResponse
    {
        public Guid StudentId { get; set; }
        public string? StudentCode { get; set; }
        public string? StudentFullName { get; set; }
    }

    public class MedicalRegistrationParentResponse
    {
        public Guid? UserId { get; set; }
        public string? UserFullName { get; set; }
    }
}
