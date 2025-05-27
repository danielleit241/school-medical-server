namespace SchoolMedicalServer.Abstractions.Dtos.Account
{
    public class RegisterStaffRequest
    {
        public string PhoneNumber { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string RoleId { get; set; } = default!;
    }
}
