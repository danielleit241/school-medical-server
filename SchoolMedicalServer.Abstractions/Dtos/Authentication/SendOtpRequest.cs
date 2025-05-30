using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Authentication
{
    public class SendOtpRequest
    {
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
