namespace SchoolMedicalServer.Abstractions.Entities
{
    public class VaccinationObservation
    {
        public Guid VaccinationObservationId { get; set; }
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

        public DateTime? CreatedAt { get; set; }

        public virtual VaccinationResult? VaccinationResult { get; set; }
    }
}
