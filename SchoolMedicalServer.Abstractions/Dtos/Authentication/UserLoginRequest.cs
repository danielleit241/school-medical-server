using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.Authentication
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Phone number is required."),
            MinLength(10, ErrorMessage = ("Phone number must be between 10 and 11 digitss")),
            MaxLength(11, ErrorMessage = ("Phone number must be between 10 and 11 digits"))]
        public string PhoneNumber { get; set; } = default!;

        [Required(ErrorMessage = "Password is required."),
            MinLength(6, ErrorMessage = ("Password must be at least 6 characters long"))]
        public string Password { get; set; } = default!;
    }
}
