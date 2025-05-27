namespace SchoolMedicalServer.Abstractions.Dtos.UserProfile
{
    public class UserProfileRequest
    {
        public string FullName { get; set; } = default!;

        public DateOnly? DateOfBirth { get; set; }

        public string? AvatarUrl { get; set; }

        public string EmailAddress { get; set; } = default!;

    }
}
