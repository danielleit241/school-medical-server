namespace SchoolMedicalServer.Abstractions.Dtos.User
{
    public class UserProfileRequest
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = default!;

        public DateOnly? DateOfBirth { get; set; }

        public string? AvatarUrl { get; set; }

        public string EmailAddress { get; set; } = default!;

        public string Address { get; set; } = string.Empty;

    }
}
