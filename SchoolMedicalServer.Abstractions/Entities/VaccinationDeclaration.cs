namespace SchoolMedicalServer.Abstractions.Entities
{
    public partial class VaccinationDeclaration
    {
        public Guid VaccinationDeclarationId { get; set; }
        public Guid HealthDeclarationId { get; set; }
        public string VaccineName { get; set; } = null!;
        public string? BatchNumber { get; set; }
        public DateOnly? VaccinatedDate { get; set; }
        public string? Notes { get; set; }
        public virtual HealthDeclaration HealthDeclaration { get; set; } = null!;
    }
}