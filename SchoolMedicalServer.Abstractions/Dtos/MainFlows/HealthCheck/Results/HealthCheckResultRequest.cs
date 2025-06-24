using System.ComponentModel.DataAnnotations;

namespace SchoolMedicalServer.Abstractions.Dtos.MainFlows.HealthCheck.Results
{
    public class HealthCheckResultRequest
    {
        [Required]
        public Guid HealthCheckResultId { get; set; }
        [Required]
        public DateOnly? DatePerformed { get; set; }
        [Required]
        public double? Height { get; set; }
        [Required]
        public double? Weight { get; set; }
        [Required]
        public double? VisionLeft { get; set; }
        [Required]
        public double? VisionRight { get; set; }
        [Required]
        public string? Hearing { get; set; }
        [Required]
        public string? Nose { get; set; }
        [Required]
        public string? BloodPressure { get; set; }
        [Required]
        public string? Status { get; set; }

        public string? Notes { get; set; }
    }
}
