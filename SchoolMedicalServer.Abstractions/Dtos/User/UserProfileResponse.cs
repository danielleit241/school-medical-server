namespace SchoolMedicalServer.Abstractions.Dtos.User
{
    public class UserProfileResponse
    {
        public string FullName { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public DateOnly? DateOfBirth { get; set; }

        public string? AvatarUrl { get; set; }

        public string EmailAddress { get; set; } = default!;

        public string Address { get; set; } = string.Empty;
    }
}
