using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration
{
    public class MedicalRegistrationNurseApprovedRequest
    {
        [Required(ErrorMessage = "Staff nurse id is required.")]
        public Guid? StaffNurseId { get; set; }

        [Required(ErrorMessage = "Staff nurse notes is required.")]
        public string? StaffNurseNotes { get; set; }

        [Required(ErrorMessage = "Date approved is required.")]
        public DateOnly? DateApproved { get; set; }
    }
}
