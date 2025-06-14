namespace SchoolMedicalServer.Abstractions.Entities
{
    public class VaccinationResult
    {
        public Guid VaccinationResultId { get; set; }
        public Guid RoundId { get; set; }
        public Guid HealthProfileId { get; set; }

        public bool? ParentConfirmed { get; set; } = null;
        public bool Vaccinated { get; set; }
        public DateOnly? VaccinatedDate { get; set; }
        public DateTime? VaccinatedTime { get; set; }
        public string? InjectionSite { get; set; }
        public Guid RecorderId { get; set; }

        public string? Status { get; set; } // "Pending", "Completed", "Failed", etc.
        public string? Notes { get; set; }

        public virtual VaccinationRound? Round { get; set; }
        public virtual HealthProfile? HealthProfile { get; set; }
        public virtual VaccinationObservation? VaccinationObservation { get; set; }
    }
}