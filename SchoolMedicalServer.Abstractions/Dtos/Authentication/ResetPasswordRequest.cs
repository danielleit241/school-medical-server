using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Authentication
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Otp { get; set; } = default!;
        [Required]
        public string PhoneNumber { get; set; } = default!;
        [Required]
        public string NewPassword { get; set; } = default!;
        [Required, Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = default!;

    }
}
