using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Authentication
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Otp is required.")]
        public string Otp { get; set; } = default!;
        [Required(ErrorMessage = "Phone number is required."),
            MinLength(10, ErrorMessage = "Phone number must be between 10 and 11 digits"),
            MaxLength(11, ErrorMessage = "Phone number must be between 10 and 11 digits")]
        public string PhoneNumber { get; set; } = default!;
        [Required]
        public string NewPassword { get; set; } = default!;
        [Required, Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = default!;

    }
}
