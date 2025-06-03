namespace SchoolMedicalServer.Abstractions.Dtos.User
{
    public class UserInformation
    {
        public Guid UserId { get; set; }

        public string? RoleName { get; set; }

        public string? FullName { get; set; }

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? AvatarUrl { get; set; }

        public DateOnly? DayOfBirth { get; set; }

        public bool? Status { get; set; }
    }
}
