namespace SchoolMedicalServer.Abstractions.Dtos.Authentication
{
    public class LoginRequest
    {
        public string PhoneNumber { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
