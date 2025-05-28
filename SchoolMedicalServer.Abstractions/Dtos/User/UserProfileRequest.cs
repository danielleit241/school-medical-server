namespace SchoolMedicalServer.Abstractions.Dtos.User
{
    public class UserProfileRequest
    {
        public string FullName { get; set; } = default!;

        public DateOnly? DateOfBirth { get; set; }

        public string? AvatarUrl { get; set; }

        public string EmailAddress { get; set; } = default!;

    }
}
