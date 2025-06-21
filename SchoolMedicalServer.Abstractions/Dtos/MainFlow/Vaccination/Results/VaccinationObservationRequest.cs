namespace SchoolMedicalServer.Abstractions.Dtos.MainFlow.Vaccination.Results
{
    public class VaccinationObservationRequest
    {
        public Guid VaccinationResultId { get; set; }

        public DateTime? ObservationStartTime { get; set; }
        public DateTime? ObservationEndTime { get; set; }

        public DateTime? ReactionStartTime { get; set; }
        public string? ReactionType { get; set; }
        public string? SeverityLevel { get; set; }
        public string? ImmediateReaction { get; set; }
        public string? Intervention { get; set; }
        public string? ObservedBy { get; set; }
        public string? Notes { get; set; }
    }
}
