using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Appointment
{
    public class AppoinmentNurseApprovedRequest
    {
        [Required]
        public Guid StaffNurseId { get; set; }
        public bool? ConfirmationStatus { get; set; }
        public bool? CompletionStatus { get; set; }
    }
}
