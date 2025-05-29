namespace SchoolMedicalServer.Abstractions.Dtos.HealthDeclaration
{
    public class HealthProfileDeclarationRequest
    {
        public HealthProfileDeclarationDtoRequest HealthDeclaration { get; set; } = default!;

        public List<VaccinationDeclarationDtoRequest>? Vaccinations { get; set; }
    }

    public class HealthProfileDeclarationDtoRequest
    {
        public DateOnly? DeclarationDate { get; set; }

        public string? ChronicDiseases { get; set; }

        public string? DrugAllergies { get; set; }

        public string? FoodAllergies { get; set; }

        public string? Notes { get; set; }
    }

    public class VaccinationDeclarationDtoRequest
    {
        public string VaccineName { get; set; } = default!;

        public string? BatchNumber { get; set; }

        public DateOnly? VaccinatedDate { get; set; }

        public string? Notes { get; set; }
    }
}
