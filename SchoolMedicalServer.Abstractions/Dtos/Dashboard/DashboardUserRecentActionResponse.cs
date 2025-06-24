namespace SchoolMedicalServer.Abstractions.Dtos.Dashboard
{
    public class DashboardUserRecentActionResponse
    {
        public UserRecentAction UserRecentAction { get; set; } = new();
    }

    public class UserRecentAction
    {
        public string? Name { get; set; }
        public DateTime? DateTime { get; set; }
        public int Count { get; set; }
    }
}
