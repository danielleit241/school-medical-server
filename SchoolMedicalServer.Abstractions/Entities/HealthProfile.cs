namespace SchoolMedicalServer.Abstractions.Entities;

public partial class HealthProfile
{
    public Guid HealthProfileId { get; set; }
    public Guid? StudentId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateOnly? DeclarationDate { get; set; }
    public string? ChronicDiseases { get; set; }
    public string? DrugAllergies { get; set; }
    public string? FoodAllergies { get; set; }
    public string? Notes { get; set; }

    public virtual Student Student { get; set; } = null!;
    public virtual ICollection<VaccinationResult> VaccinationResults { get; set; } = new List<VaccinationResult>();
    public virtual ICollection<HealthCheckResult> HealthCheckResults { get; set; } = new List<HealthCheckResult>();
    public virtual ICollection<VaccinationDeclaration> VaccinationDeclarations { get; set; } = new List<VaccinationDeclaration>();
}
