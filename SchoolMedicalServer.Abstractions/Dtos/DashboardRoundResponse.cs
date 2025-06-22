namespace SchoolMedicalServer.Abstractions.Dtos
{
    public class DashboardRoundResponse
    {
        public string? RoundName { get; set; }
        public int Daylefts { get; set; }
        public DateOnly? StartDate { get; set; }
    }
}
