namespace SchoolMedicalServer.Abstractions.Entities
{
    public partial class VaccinationDeclaration
    {
        public Guid VaccinationDeclarationId { get; set; }
        public Guid HealthProfileId { get; set; }
        public string VaccineName { get; set; } = null!;
        public string? BatchNumber { get; set; }
        public DateOnly? VaccinatedDate { get; set; }
        public virtual HealthProfile HealthProfile { get; set; } = null!;
    }
}