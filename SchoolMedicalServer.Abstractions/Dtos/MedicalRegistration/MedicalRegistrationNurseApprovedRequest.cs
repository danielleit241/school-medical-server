using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration
{
    public class MedicalRegistrationNurseApprovedRequest
    {

        [Required(ErrorMessage = "Staff nurse id is required.")]
        public Guid? StaffNurseId { get; set; }

        [Required(ErrorMessage = "Date approved is required.")]
        public DateOnly? DateApproved { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public bool Status { get; set; }

        public string? NurseNotes { get; set; }
    }
}
