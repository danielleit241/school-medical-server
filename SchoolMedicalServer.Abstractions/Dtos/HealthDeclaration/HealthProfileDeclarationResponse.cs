namespace SchoolMedicalServer.Abstractions.Dtos.HealthDeclaration
{
    public class HealthProfileDeclarationResponse
    {
        public HealthProfileDeclarationDtoResponse HealthDeclaration { get; set; } = default!;

        public List<VaccinationDeclarationDtoResponse>? Vaccinations { get; set; }
    }

    public class HealthProfileDeclarationDtoResponse
    {
        public Guid HealthProfileId { get; set; }

        public Guid? StudentId { get; set; }

        public DateOnly? DeclarationDate { get; set; }

        public string? ChronicDiseases { get; set; }

        public string? DrugAllergies { get; set; }

        public string? FoodAllergies { get; set; }

        public string? Notes { get; set; }
        public bool IsDeclaration { get; set; }
    }

    public class VaccinationDeclarationDtoResponse
    {
        public string VaccineName { get; set; } = default!;

        public string? DoseNumber { get; set; }

        public DateOnly? VaccinatedDate { get; set; }

        public string? Notes { get; set; }
    }
}
