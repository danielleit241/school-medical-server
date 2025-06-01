using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Authentication
{
    public class ChangePasswordRequest
    {
        [Required, MinLength(10, ErrorMessage = "Phone number must be between 10 and 11 digits"),
            MaxLength(11, ErrorMessage = "Phone number must be between 10 and 11 digits")]
        public string PhoneNumber { get; set; } = default!;
        [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string OldPassword { get; set; } = default!;
        [Required, MinLength(6, ErrorMessage = "New password must be at least 6 characters long")]
        public string NewPassword { get; set; } = default!;
        [Required, MinLength(6, ErrorMessage = "Confirm new password must be at least 6 characters long"),
        Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = default!;
    }
}
