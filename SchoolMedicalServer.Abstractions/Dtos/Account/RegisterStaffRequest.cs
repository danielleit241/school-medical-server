using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Account
{
    public class RegisterStaffRequest
    {
        [Required, MinLength(10), MaxLength(11)]
        public string PhoneNumber { get; set; } = default!;
        [Required]
        public string FullName { get; set; } = default!;
        [Required, EmailAddress]
        public string Email { get; set; } = default!;
        [Required, MinLength(6), MaxLength(24)]
        public string Password { get; set; } = default!;
        [Required]
        public string RoleName { get; set; } = default!;
    }
}
