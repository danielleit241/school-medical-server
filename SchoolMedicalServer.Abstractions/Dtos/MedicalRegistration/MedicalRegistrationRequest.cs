namespace SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration
{
    public class MedicalRegistrationRequest
    {
        public Guid StudentId { get; set; }
        public Guid UserId { get; set; }
        public DateOnly? DateSubmitted { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool ParentConsent { get; set; }
    }
}
