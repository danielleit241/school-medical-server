using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Authentication
{
    public class SendOtpRequest
    {
        [Required(ErrorMessage = "Phone number is required."),
            MinLength(10, ErrorMessage = "Phone number must be between 10 and 11 digits"),
            MaxLength(11, ErrorMessage = "Phone number must be between 10 and 11 digits")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
