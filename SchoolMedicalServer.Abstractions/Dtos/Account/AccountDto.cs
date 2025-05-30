namespace SchoolMedicalServer.Abstractions.Dtos.Account
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string EmailAddress { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
