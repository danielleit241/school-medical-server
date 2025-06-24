namespace SchoolMedicalServer.Abstractions.Dtos.Dashboard
{
    public class DashboardRequest
    {
        public DateOnly? From { get; set; } = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        public DateOnly? To { get; set; } = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(1).AddDays(-1);

        public DashboardRequest()
        {
        }
    }
}
