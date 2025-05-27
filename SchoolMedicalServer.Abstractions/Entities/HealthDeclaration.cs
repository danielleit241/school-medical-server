namespace SchoolMedicalServer.Abstractions.Entities;

public partial class HealthDeclaration
{
    public Guid HealthDeclarationId { get; set; }

    public Guid? StudentId { get; set; }

    public DateOnly? DeclarationDate { get; set; }

    public string? ChronicDiseases { get; set; }

    public string? DrugAllergies { get; set; }

    public string? FoodAllergies { get; set; }

    public string? Notes { get; set; }

    public virtual Student? Student { get; set; }
    public virtual ICollection<VaccinationDeclaration> VaccinationDeclarations { get; set; } = new List<VaccinationDeclaration>();
}
