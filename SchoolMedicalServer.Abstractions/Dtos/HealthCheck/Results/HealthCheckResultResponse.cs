namespace SchoolMedicalServer.Abstractions.Dtos.HealthCheck.Results
{
    public class HealthCheckResultResponse
    {
        public Guid ResultId { get; set; }
        public Guid HealthProfileId { get; set; }
        public DateOnly? DatePerformed { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public double? VisionLeft { get; set; }
        public double? VisionRight { get; set; }
        public string? Hearing { get; set; }
        public string? Nose { get; set; }
        public string? BloodPressure { get; set; }
        public bool? Status { get; set; }
        public string? Notes { get; set; }
        public NurseInformationResponse? RecordedBy { get; set; }
    }

    public class NurseInformationResponse
    {
        public Guid NurseId { get; set; }
        public string? NurseName { get; set; }
        public string? NursePhone { get; set; }
    }
}
