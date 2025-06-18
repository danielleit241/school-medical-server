namespace SchoolMedicalServer.Abstractions.Dtos.Vaccination.Results
{
    public class VaccinationResultResponse
    {
        public VaccinationResultInformationResponse ResultResponse { get; set; } = default!;
        public VaccinationObservationInformationResponse? VaccinationObservation { get; set; }
    }

    public class VaccinationResultInformationResponse
    {
        public Guid VaccinationResultId { get; set; }
        public Guid RecorderId { get; set; }
        public Guid HealthProfileId { get; set; }
        public Guid VaccineId { get; set; }
        public DateOnly? VaccinatedDate { get; set; }
        public DateTime? VaccinatedTime { get; set; }
        public bool Vaccinated { get; set; }
        public string? InjectionSite { get; set; }
        public bool? ParentConfirmed { get; set; }
        public bool? HealthQualified { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }

    public class VaccinationObservationInformationResponse
    {
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
