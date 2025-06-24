using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows
{
    public class ParentConfirmationRequest
    {
        [Required]
        public bool Status { get; set; }
    }
}
