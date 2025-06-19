namespace SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Results
{
    public class HealthCheckResultRequest
    {
        public Guid HealthCheckResultId { get; set; }
        public DateOnly? DatePerformed { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public double? VisionLeft { get; set; }
        public double? VisionRight { get; set; }
        public string? Hearing { get; set; }
        public string? Nose { get; set; }
        public string? BloodPressure { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
}
