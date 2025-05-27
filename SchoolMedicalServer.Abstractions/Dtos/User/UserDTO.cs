namespace SchoolMedicalServer.Abstractions.Dtos.User
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        public string? RoleName { get; set; }

        public string? FullName { get; set; }

        public string? EmailAddress { get; set; }

        public string? AvatarUrl { get; set; }

        public DateOnly? DayOfBirth { get; set; }

        public bool? Status { get; set; }
    }
}
